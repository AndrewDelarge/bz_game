using game.core.InputSystem;

namespace game.Gameplay.Characters {
	public class PlayerActionIdleState : PlayerStateBase<PlayerActionState> {
		public override void HandleState() {
		}

		public override void HandleInput(InputData data) {
			var kick = data.GetAction(InputActionType.KICK);

			if (kick is {value: {status: InputStatus.DOWN}}) {
				kick.isAbsorbed = true;
				context.mainStateMachine.ChangeState(CharacterStateEnum.WALK);
				context.actionStateMachine.ChangeState(PlayerActionState.KICK);
			}
		}
	}
}