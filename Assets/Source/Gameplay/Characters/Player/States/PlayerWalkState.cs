using game.core.InputSystem;
using UnityEngine;

namespace game.Gameplay.Characters.Player {
	public class PlayerWalkState : PlayerStateBase<CharacterStateEnum> {
		protected Vector2 _move;
		protected float _currentSpeedMultiplier = 1f;

		public override void HandleState() {
			var direction = _move.normalized;
			var angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + context.camera.transform.eulerAngles.y;
			var moveDirection = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;

			float characterSpeed = context.data.normalSpeed * GetSpeedMultiplier();

			var move = new CharacterMove(moveDirection, characterSpeed, angle);

			context.movement.Move(move);

			context.animation.SetMotionVelocityPercent(context.movement.GetHorizontalVelocity() /
			                                           (context.data.normalSpeed * context.data.speedMultiplier));
		}

		public override void HandleInput(InputData data) {
			_move = data.move.isAbsorbed ? Vector2.zero : data.move.value;

			if (_move == Vector2.zero) {
				context.mainStateMachine.ChangeState(CharacterStateEnum.IDLE);
				return;
			}
			
			var sprint = data.GetAction(InputActionType.SPRINT);


			if (sprint is {value: {status: InputStatus.DOWN}}) {
				context.mainStateMachine.ChangeState(CharacterStateEnum.RUN);
				return;
			}
		}

		protected virtual float GetSpeedMultiplier() {
			return _currentSpeedMultiplier = 1;
		}
	}
}