using System.Collections.Generic;
using System.Linq;
using game.core.InputSystem;
using game.core.Storage.Data.Character;
using game.Gameplay.Characters.Player;
using UnityEngine;

namespace game.Gameplay.Characters.AI {
	public class AIDeadState : CharacterState<CharacterStateEnum, CharacterContext> {
		private List<Rigidbody> _ragdollBones;
		private float _endTime;

		public override void Init(CharacterContext context) {
			base.Init(context);
			
			_ragdollBones = context.transform.GetComponentsInChildren<Rigidbody>().ToList();
			
			SetRagdollValue(true);
		}

		public override void Enter() {
			var animData = context.animation.GetAnimationData(CharacterAnimationEnum.KICK);
			_endTime = animData.clip.length * 0;

			context.animation.PlayAnimation(animData.clip);
		}

		public override void Exit() {
			throw new System.NotImplementedException();
		}

		public override void HandleState(float deltaTime) {
			_endTime -= Time.deltaTime;
			
			if (_endTime <= 0) {
				context.animation.Disable();
				context.movement.Disable();
			
				SetRagdollValue(false);
			}
		}

		public void HandleInput(InputData data) {}

		private void SetRagdollValue(bool value) {
			foreach (var bone in _ragdollBones) {
				bone.isKinematic = value;
			}
		}
	}
}