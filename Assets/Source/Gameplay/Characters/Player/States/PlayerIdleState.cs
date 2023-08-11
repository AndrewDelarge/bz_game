using game.core.InputSystem;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace game.Gameplay.Characters.Player {
	public class PlayerIdleState : PlayerStateBase<CharacterStateEnum, PlayerCharacterContext> {
		public override void HandleState(float deltaTime) {

			
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