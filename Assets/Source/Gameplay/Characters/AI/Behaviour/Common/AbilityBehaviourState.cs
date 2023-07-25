using game.Gameplay.Characters.Common.Abilities;

namespace game.Gameplay.Characters.AI.Behaviour {
	public abstract class AbilityBehaviourState : BaseBehaviourState {
		public override BehaviourState type => BehaviourState.ABILITY;

		protected IAbility _ability;
		public virtual void SetAbility(IAbility ability) {
			_ability = ability;
		}

		public override void Enter() {
		}

		public override void HandleState(float deltaTime) {
			base.HandleState(deltaTime);
		}
	}
}