using game.Source.core.Common;

namespace game.Gameplay.Characters.AI.Behaviour {
	public class TestSpecialAbilityBehaviourState : AbilityBehaviourState {
		public override bool CheckEnterCondition() => _context.character.canChangeState;

		public override void Enter() {
			_context.character.animator.SetMotionVelocityPercent(0, true);
			
			_ability.SetTarget(_context.target);
			_ability.Use();

			AppCore.Get<GameTimer>().SetTimeout(_ability.abilityTime, _context.stateMachine.ReturnState);
		}

	}
}