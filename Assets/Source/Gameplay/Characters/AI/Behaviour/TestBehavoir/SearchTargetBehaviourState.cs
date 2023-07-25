using game.core;
using game.Gameplay.Characters.Common;
using UnityEngine;

namespace game.Gameplay.Characters.AI.Behaviour {
	public class SearchTargetBehaviourState : BaseBehaviourState
	{
		private const int AGRODISTANCE = 10;
		private BehaviourContext _context;
		private CharactersManager _charactersManager;
		private ICharacter _player;

		public override BehaviourState type => BehaviourState.TARGET_SEARCHING;

		public override void Init(BehaviourContext context)
		{
			_context = context;
			_charactersManager = AppCore.Get<CharactersManager>();
			_player = _charactersManager.player;
		}

		public override void HandleState(float deltaTime)
		{
			if (Vector3.Distance(_player.currentPosition, _context.character.currentPosition) > AGRODISTANCE) {
				return;
			}
            
			_context.target = _charactersManager.player;

			_context.stateMachine.ChangeState(BehaviourState.TARGET_FOLLOW);
		}
	}
}