using game.core.InputSystem;
using UnityEngine;

namespace game.Gameplay.Characters.Player {
	public class PlayerIdleState : PlayerStateBase<CharacterStateEnum, PlayerCharacterContext> {
		public override void HandleState() {
			context.animation.SetMotionVelocityPercent(context.movement.GetHorizontalVelocity() /
			                                           (context.data.normalSpeed * context.data.speedMultiplier));
		}

		public override void HandleInput(InputData data) {
			if (data.move.value != Vector2.zero && data.move.isAbsorbed == false) {
				context.mainStateMachine.ChangeState(CharacterStateEnum.WALK);
			}
		}
	}
}