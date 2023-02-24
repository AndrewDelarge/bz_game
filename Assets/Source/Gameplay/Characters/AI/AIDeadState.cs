using System.Collections.Generic;
using System.Linq;
using game.core.InputSystem;
using game.core.Storage.Data.Character;
using game.Gameplay.Characters.Player;
using UnityEngine;

namespace game.Gameplay.Characters.AI {
	public class AIDeadState : CharacterState<CharacterStateEnum> {
		private List<Rigidbody> _ragdollBones;
		private float _endTime;

		public override void Init(CharacterContext context) {
			base.Init(context);
			
			_ragdollBones = context.transform.GetComponentsInChildren<Rigidbody>().ToList();
			
			SetRagdollValue(true);
		}

		public override void Enter() {
			var animData = context.characterAnimationSet.GetAnimationData(CharacterAnimationEnum.KICK);
			_endTime = animData.clip.length * .8f;

			context.animation.PlayAnimation(animData.clip);
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