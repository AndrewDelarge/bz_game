using System.Collections.Generic;
using System.Linq;
using game.core.InputSystem;
using UnityEngine;

namespace game.Source.Gameplay.Characters.AI {
	public class AIDeadState : CharacterState<CharacterStateEnum> {
		private List<Rigidbody> _ragdollBones;
		private float _endTime;

		public override void Init(CharacterContext context) {
			base.Init(context);
			
			_ragdollBones = context.transform.GetComponentsInChildren<Rigidbody>().ToList();
			
			SetRagdollValue(true);
		}

		public override void Enter() {
			_endTime = context.animationSet.testClip.length * .8f;

			context.animation.PlayAnimation(context.animationSet.testClip);
		}

		public override void Exit() {
			throw new System.NotImplementedException();
		}

		public override void HandleState() {
			_endTime -= Time.deltaTime;
			
			if (_endTime <= 0) {
				context.animation.Disable();
				context.movement.Disable();
			
				SetRagdollValue(false);
			}
		}

		public override void HandleInput(InputData data) {}

		private void SetRagdollValue(bool value) {
			foreach (var bone in _ragdollBones) {
				bone.isKinematic = value;
			}
		}
	}
}