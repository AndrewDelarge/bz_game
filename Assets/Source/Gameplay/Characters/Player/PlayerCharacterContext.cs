using game.core.Storage.Data.Character;
using game.gameplay.characters;
using game.Source.Gameplay.Characters.Common;
using UnityEngine;

namespace game.Source.Gameplay.Characters {
	public class PlayerCharacterContext : CharacterContext {
		public Camera camera;
		public PlayerStateBase idleState;
		public PlayerStateBase moveState;

		public PlayerStateBase idleActionState;
		public PlayerStateBase kickActionState;
		public PlayerCharacterContext(CharacterMovement movement, CharacterAnimation animation, CharacterAnimData animationData, PlayerCommonData data, Transform transform, BaseStateMachine actionStateMachine, BaseStateMachine mainStateMachine) : base(movement, animation, animationData, data, transform, actionStateMachine, mainStateMachine) {
		}
	}
}