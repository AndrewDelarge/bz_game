using game.core.InputSystem;

namespace game.Source.Gameplay.Characters.AI {
	public abstract class BaseAICharacterState : CharacterState {
		public override void Enter() {
			context.healthable.die += CharacterDied;
		}

		public override void Exit() {
			context.healthable.die -= CharacterDied;
		}

		public override void HandleInput(InputData data) {
			
		}

		public virtual void CharacterDied() {
			context.mainStateMachine.ChangeState(context.states[typeof(AIDeadState)]);
		}
	}
}