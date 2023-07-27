using game.Source.core.Common;

namespace game.Gameplay.Characters.AI.Behaviour {
	public class TestSpecialAbilityBehaviourState : AbilityBehaviourState {
		public override void Enter() {
			_ability.SetTarget(_context.target);
			_ability.Use();

			AppCore.Get<GameTimer>().SetTimeout(_ability.abilityTime, _context.stateMachine.ReturnState);
		}

	}
}