using game.core.InputSystem;
using UnityEngine;

namespace game.Source.Gameplay.Characters {
	public class PlayerMoveState : PlayerStateBase<CharacterStateEnum> {
		private bool _sprint;
		private Vector2 _move;
		private float _currentSprintMultiplier = 1f;

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

			_sprint = false;

			if (sprint != null && sprint.value.status == InputStatus.PRESSED) {
				_sprint = true;
			}

			var kick = data.GetAction(InputActionType.KICK);

			if (kick != null && kick.value.status == InputStatus.DOWN && _sprint == false) {
				kick.isAbsorbed = true;
				context.actionStateMachine.ChangeState(PlayerActionState.KICK);
			}
		}

		private float GetSpeedMultiplier() {
			if (!_sprint) {
				_currentSprintMultiplier = 1;
			}
			else {
				_currentSprintMultiplier = Mathf.Lerp(_currentSprintMultiplier, context.data.speedMultiplier,
					context.data.speedSmoothTime * Time.deltaTime);
			}

			return _currentSprintMultiplier;
		}
	}
}