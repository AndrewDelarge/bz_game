using game.core;
using game.core.level;
using game.Gameplay.Characters.Common;
using UnityEngine;

namespace game.Gameplay.Characters.AI.Behaviour {
	public class SearchTargetBehaviourState : BaseBehaviourState
	{
		private const int AGRODISTANCE = 10;
		private BehaviourContext _context;
		private CharactersController _charactersController;
		private ICharacter _player;

		public override BehaviourState type => BehaviourState.TARGET_SEARCHING;

		public override void Init(BehaviourContext context)
		{
			_context = context;
			_charactersController = AppCore.Get<LevelManager>().characterController;
			_player = _charactersController.player;
		}

		public override void HandleState(float deltaTime)
		{
			if (_player.healthable.isDead) {
				return;
			}
			
			if (Vector3.Distance(_player.currentPosition, _context.character.currentPosition) > AGRODISTANCE) {
				return;
			}
            
			_context.target = _charactersController.player;

			_context.stateMachine.ChangeState(BehaviourState.TARGET_FOLLOW);
		}
	}
}