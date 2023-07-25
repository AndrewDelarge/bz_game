using game.core.InputSystem;
using game.Gameplay.Characters.Common.Abilities;

namespace game.Gameplay.Characters.AI {
	public class AIUseAbilityState : BaseAICharacterState {
		private IAbility _ability;
		public void SetAbility(IAbility ability) {
			_ability = ability;
		}
		
		public override void HandleState(float deltaTime) {
			if (_ability.isUsing) {
				return;
			}

			context.mainStateMachine.ChangeState(CharacterStateEnum.IDLE);
		}

		public override void HandleInput(InputData data) {
			_ability.HandleInput(data);
		}
	}
}