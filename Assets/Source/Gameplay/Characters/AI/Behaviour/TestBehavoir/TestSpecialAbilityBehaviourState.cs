namespace game.Gameplay.Characters.AI.Behaviour {
	public class TestSpecialAbilityBehaviourState : AbilityBehaviourState {
		private float _tempTimer;

		public override void Enter() {
			_tempTimer = _ability.abilityTime;
			
			_ability.SetTarget(_context.target);
			_ability.Use();
		}

		public override void HandleState(float deltaTime) {
			if (_tempTimer > 0) {
				_tempTimer -= deltaTime;
				return;
			}
			
			_context.stateMachine.ReturnState();
		}
	}
}