using game.core.InputSystem;
using UnityEngine;

namespace game.Gameplay.Characters.Player {
	public class PlayerRunState : PlayerWalkState {
		public override void Enter() {
			base.Enter();
			
			_currentSpeedMultiplier = 1;
		}

		public override void HandleInput(InputData data) {
			_move = data.move.isAbsorbed ? Vector2.zero : data.move.value;

			var sprint = data.GetAction(InputActionType.SPRINT);
			
			if (sprint is not {value: {status: InputStatus.PRESSED}} || _move == Vector2.zero) {
				context.mainStateMachine.ChangeState(CharacterStateEnum.WALK);
				return;
			}

			
		}
		
		protected override float GetSpeedMultiplier() {
			return _currentSpeedMultiplier = Mathf.Lerp(_currentSpeedMultiplier, context.data.speedMultiplier,
				context.data.speedSmoothTime * Time.deltaTime);
		}
	}
}