using game.core.InputSystem;
using game.Gameplay.Characters.Common;

namespace game.Gameplay.Characters.Player {
	public class PlayerActionIdleState : PlayerStateBase<PlayerActionStateEnum> {
		public override void HandleState() {
		}

		public override void HandleInput(InputData data) {
			var kick = data.GetAction(InputActionType.KICK);

			if (kick is {value: {status: InputStatus.DOWN}}) {
				kick.isAbsorbed = true;
				context.actionStateMachine.ChangeState(PlayerActionStateEnum.KICK);
			}
		}
	}
}