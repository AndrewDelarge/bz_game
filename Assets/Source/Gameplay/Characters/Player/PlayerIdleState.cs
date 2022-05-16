using game.core.InputSystem;
using UnityEngine;

namespace game.Source.Gameplay.Characters {
	public class PlayerIdleState : PlayerStateBase {
		public PlayerIdleState(PlayerCharacterContext context) : base(context) {
		}

		public override void HandleState() {
			context.animation.SetMotionVelocityPercent(context.movement.GetHorizontalVelocity() /
			                                           (context.data.normalSpeed * context.data.speedMultiplier));
		}

		public override void HandleInput(InputData data) {
			InputActionField<InputAction> kick = data.GetAction(InputActionType.KICK);

			if (kick != null && kick.value.status == InputStatus.UP) {
				kick.isAbsorbed = true;
				context.actionStateMachine.ChangeState(context.kickActionState);
			}

			if (data.move.value != Vector2.zero && data.move.isAbsorbed == false) {
				context.mainStateMachine.ChangeState(context.moveState);
			}
		}
	}
}