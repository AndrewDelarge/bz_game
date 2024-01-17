using System.Collections.Generic;
using game.core.InputSystem;
using game.core.storage;
using game.core.Common.Helpers;
using game.core.Storage.Data.Character;
using game.Source.core.Common;
using UnityEngine;

namespace game.Gameplay.Characters.Player {
	public class PlayerActionKickState : PlayerStateBase<PlayerActionStateEnum, PlayerCharacterContext> {
		private List<int> _timers = new List<int>();
		private GameTimer _timer;
		public override void Init(PlayerCharacterContext context) {
			base.Init(context);
			
			_timer = AppCore.Get<GameTimer>();
		}

		public override void HandleInput(InputData data) {
			data.move.isAbsorbed = true;
		}

		public override void Enter() {
			base.Enter();
			context.movement.ForceStop();
			context.animation.SetMotionVelocityPercent(0f, true);
			
			if (context.mainStateMachine.currentState != CharacterStateEnum.IDLE) {
				var id = _timer.SetTimeout(.1f, StartKick);
				_timers.Add(id);
				return;
			}

			StartKick();
		}

		public override void Exit() {
			foreach (var timer in _timers) {
				_timer.KillTimeout(timer);
			}
		}

		private void StartKick() {
			_timer.SetTimeout(context.data.kickPhysicsImpulseDelay, ProduceKick);
			context.animation.PlayAnimation(CharacterAnimationEnum.KICK);
			context.animation.onAnimationComplete.Add(OnAnimationComplete);
		}

		private void ProduceKick() {
			ProduceDamage();
			ProduceKickImpulse();
		}
		
		private void OnAnimationComplete(CharacterAnimationEnum obj) {
			context.actionStateMachine.ChangeState(PlayerActionStateEnum.IDLE);
			context.animation.onAnimationComplete.Remove(OnAnimationComplete);
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