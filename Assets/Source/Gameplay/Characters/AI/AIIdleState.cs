using game.Gameplay.Characters.Player.Common;

namespace game.Gameplay.Characters.AI {
	public class AIIdleState : BaseAICharacterState {
		public override void HandleState() {
			context.animation.SetMotionVelocityPercent(context.movement.GetHorizontalVelocity() /
			                                           (context.data.normalSpeed * context.data.speedMultiplier));
		}
	}
}