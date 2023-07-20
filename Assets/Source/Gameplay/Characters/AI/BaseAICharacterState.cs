using System;
using game.core.InputSystem;
using game.Gameplay.Characters.Player;

namespace game.Gameplay.Characters.AI {
	public abstract class BaseAICharacterState : CharacterState<CharacterStateEnum, CharacterContext> {
		public override void Enter() {
			context.healthable.die.Add(CharacterDied);
		}

		public override void Exit() {
			context.healthable.die.Remove(CharacterDied);
		}

		public virtual void HandleInput(InputData data) {
			
		}

		public virtual void CharacterDied() {
			context.mainStateMachine.ChangeState(CharacterStateEnum.DEAD);
		}
	}
}