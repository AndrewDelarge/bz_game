using System.Collections.Generic;
using System.Linq;
using game.core.Storage.Data.Character;
using game.Source.core.Common;
using UnityEngine;

namespace game.Gameplay.Characters.AI {
	public class AIDeadState : CharacterState<CharacterStateEnum, CharacterContext> {
		private List<Rigidbody> _ragdollBones;
		private float _endTime;
		public override bool CheckExitCondition() => false;

		public override void Init(CharacterContext context) {
			base.Init(context);
			
			_ragdollBones = context.transform.GetComponentsInChildren<Rigidbody>().ToList();
			
			SetRagdollValue(true);
		}

		public override void Enter() {
			var animData = context.animation.GetAnimationData(CharacterAnimationEnum.KICK);
			AppCore.Get<GameTimer>().SetTimeout(_endTime, OnDie);

			context.animation.PlayAnimation(animData.clip);
		}

		private void OnDie() {
			context.animation.Disable();
			context.movement.Disable();
			
			SetRagdollValue(false);
		}

		private void SetRagdollValue(bool value) {
			foreach (var bone in _ragdollBones) {
				bone.isKinematic = value;
			}
		}
	}
}