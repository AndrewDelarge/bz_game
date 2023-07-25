using UnityEngine;

namespace game.Gameplay.Characters.AI.Behaviour {
	public class AttackBehaviourState : BaseBehaviourState {
		private const int ATTACK_RANGE = 1;
		public override BehaviourState type => BehaviourState.ATTACK;

		public override void HandleState(float deltaTime) {
			if (_context.target == null) {
				_context.stateMachine.ChangeState(BehaviourState.TARGET_SEARCHING);
				return;
			}
			if (Vector3.Distance(_context.character.currentPosition, _context.target.currentPosition) > ATTACK_RANGE) {
				_context.stateMachine.ChangeState(BehaviourState.TARGET_FOLLOW);
				return;
			}
			
			var ability = _context.character.abilitySystem.GetBestFreeAbility();
			
			if (ability?.behaviourState != null) {
				_context.stateMachine.ChangeState(ability.behaviourState);
			}
		}
		
		public override void Enter() {
			_context.target?.healthable.die.Add(OnTargetDie);
		}

		public override void Exit() {
			_context.target?.healthable.die.Remove(OnTargetDie);
		}

		private void OnTargetDie() {
			_context.target = null;
		}
	}
}