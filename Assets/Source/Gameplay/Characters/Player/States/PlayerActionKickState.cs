using game.core.InputSystem;
using game.core.storage;
using game.core.Common.Helpers;
using game.core.Storage.Data.Character;
using UnityEngine;

namespace game.Gameplay.Characters.Player {
	public class PlayerActionKickState : PlayerStateBase<PlayerActionStateEnum, PlayerCharacterContext> {
		private float _endTime;
		private float _impulsTime;
		

		public override void HandleState() {
			_endTime -= Time.deltaTime;
			if (_endTime <= 0) {
				context.actionStateMachine.ChangeState(PlayerActionStateEnum.IDLE);
			}

			if (_endTime <= _impulsTime) {
				ProduceDamage();
				ProduceKickImpulse();

				_impulsTime = int.MinValue;
			}
		}

		public override void HandleInput(InputData data) {
			data.move.isAbsorbed = true;
		}

		public override void Enter() {
			base.Enter();
			var animData = context.animation.GetAnimationData(CharacterAnimationEnum.KICK);
			_endTime = animData.length * .9f;
			_impulsTime = _endTime - context.data.kickPhysicsImpulseDelay;
			context.animation.PlayAnimation(animData.clip);
		}

		private void ProduceDamage() {
			float distance = context.data.kickFlightSphereDistance;
			float radius = context.data.kickSphereRadius;

			Vector3 centerPos = context.transform.position;
			Vector3 charForward = context.transform.forward;

			RaycastHit[] raycastHits = new RaycastHit[15];
			Physics.SphereCastNonAlloc(centerPos, radius, charForward, raycastHits, distance, (int) GameLayers.HEALTHABLE_OBJECTS);

			foreach (RaycastHit hit in raycastHits) {
				if (hit.transform == null) {
					continue;
				}

				Healthable healthable = hit.transform.gameObject.GetComponent<Healthable>();

				if (healthable == null) {
					continue;
				}

				healthable.TakeDamage(new HealthChange<DamageType>(10, DamageType.PHYSICS));
			}
		}

		private void ProduceKickImpulse() {
			float distance = context.data.kickFlightSphereDistance;
			float radius = context.data.kickSphereRadius;
			float viewAngle = context.data.kickAngle;
			float kickPower = context.data.kickPower;

			Vector3 centerPos = context.transform.position;
			Vector3 charForward = context.transform.forward;

			RaycastHit[] raycastHits = new RaycastHit[15];
			Physics.SphereCastNonAlloc(centerPos, radius, charForward, raycastHits, distance, (int) GameLayers.PHYSICS_OBJECTS);

			for (int i = 0; i < raycastHits.Length; i++) {
				Rigidbody rigidbody = raycastHits[i].rigidbody;

				if (rigidbody != null && VectorHelper.IsInViewAngle(context.transform, rigidbody.transform.position, viewAngle)) {
					Vector3 direction = charForward;
					direction.y = context.data.yKick;
					rigidbody.AddForceAtPosition(direction * kickPower, centerPos + charForward, ForceMode.Impulse);
				}
			}
		}
	}
}