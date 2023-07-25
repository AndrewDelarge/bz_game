using game.core;
using game.Gameplay.Characters.AI.Common;
using UnityEngine;

namespace game.Gameplay.Characters.AI.Behaviour {
	public class ChasingTargetBehaviourState : BaseBehaviourState {
		private const float DISAGRO_DISTANCE = 10f;
		private const float LAST_TARGET_POS_DELTA = 2f;
		private const float UPDATE_TICK = 0.5f;
		private const float THRESHOLD = 1f;

		private INavigator _navigator;
		private AICharacterControl _control = new ();
		
		private float _lastUpdate;
		private Vector3 _lastTargetPosition;

		public override BehaviourState type => BehaviourState.TARGET_FOLLOW;

		public override void Init(BehaviourContext context)
		{
			base.Init(context);
			
			_navigator = AppCore.Get<LevelManager>().navigator;
			_control.Init(_context.character.controlable);
		}

		public override void HandleState(float deltaTime)
		{
			_control.Update(deltaTime);
			
			if (_context.target == null || IsTargetToFar()) {
				_control.StopFollow();
				_context.stateMachine.ChangeState(BehaviourState.TARGET_SEARCHING);
				return;
			}

			if (_lastUpdate > 0) {
				_lastUpdate -= deltaTime;
				return;
			}
            
			if (IsPathActual() == false) {
				_control.StopFollow();
			}

			if (_control.hasTarget) {
				return;
			}

			_lastUpdate = UPDATE_TICK;

			var path = _navigator.GetPath(_context.character.currentPosition, _context.target.currentPosition);
            
			if (path == null) {
				return;
			}

			_lastTargetPosition = _context.target.currentPosition;
			_control.FollowPath(path);
			_control.followComplete.Add(OnFollowComplete);
		}

		public override void Exit() {
			_control.StopFollow();
		}

		private void OnFollowComplete() {
			_context.stateMachine.ChangeState(BehaviourState.ATTACK);
		}


		private bool IsTargetToFar() => Vector3.Distance(_context.character.currentPosition, _context.target.currentPosition) > DISAGRO_DISTANCE;

		private bool IsPathActual() => Vector3.Distance(_lastTargetPosition, _context.target.currentPosition) > LAST_TARGET_POS_DELTA;
	}
}