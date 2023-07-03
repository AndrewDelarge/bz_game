using game.core.InputSystem;
using game.Gameplay.Characters;
using UnityEngine;

namespace game.Gameplay.Characters.AI {
	public class AIWalkState : BaseAICharacterState
	{
		protected Vector2 _move;
		protected float _currentSpeedMultiplier = 1f;

		public override void HandleState() {
			var direction = _move.normalized;
			var angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + context.transform.eulerAngles.y;
			
			float characterSpeed = context.data.normalSpeed * GetSpeedMultiplier();

			var move = new CharacterMove(_move, characterSpeed, angle);

			context.movement.Move(move);

			context.animation.SetMotionVelocityPercent(context.movement.GetHorizontalVelocity() /
			                                           (context.data.normalSpeed * context.data.speedMultiplier));
		}

		public override void HandleInput(InputData data) {
			_move = data.move.isAbsorbed ? Vector2.zero : data.move.value;

			if (_move == Vector2.zero) {
				context.mainStateMachine.ChangeState(CharacterStateEnum.IDLE);
			}
		}

		protected virtual float GetSpeedMultiplier() {
			return _currentSpeedMultiplier = 1;
		}
	}
	public class AIIdleState : BaseAICharacterState {
		public override void HandleInput(InputData data) {
			if (data.move.value != Vector2.zero && data.move.isAbsorbed == false) {
				context.mainStateMachine.ChangeState(CharacterStateEnum.WALK);
			}
		}

		public override void HandleState() {
			context.animation.SetMotionVelocityPercent(context.movement.GetHorizontalVelocity() /
			                                           (context.data.normalSpeed * context.data.speedMultiplier));
		}
	}
}