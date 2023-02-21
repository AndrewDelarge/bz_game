using game.core.InputSystem;
using game.Gameplay.Characters.Player;

namespace game.Gameplay.Characters.AI {
	public abstract class BaseAICharacterState : CharacterState<CharacterStateEnum> {
		public override void Enter() {
			context.healthable.die += CharacterDied;
		}

		public override void Exit() {
			context.healthable.die -= CharacterDied;
		}

		public override void HandleInput(InputData data) {
			
		}

		public virtual void CharacterDied() {
			context.mainStateMachine.ChangeState(CharacterStateEnum.DEAD);
		}
	}
}