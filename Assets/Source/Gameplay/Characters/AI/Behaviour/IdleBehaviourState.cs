namespace game.Gameplay.Characters.AI.Behaviour {

	public class TestSpecialAbilityBehaviourState : BaseBehaviourState {
		public override BehaviourState type => BehaviourState.ABILITY;


		public override void HandleState(float deltaTime) {
			base.HandleState(deltaTime);
		}
	}
	public class IdleBehaviourState : BaseBehaviourState
	{
		public override BehaviourState type => BehaviourState.IDLE;
	}
}