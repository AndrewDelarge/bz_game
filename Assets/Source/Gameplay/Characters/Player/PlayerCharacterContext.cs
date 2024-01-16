using game.core.Storage.Data.Character;
using game.Gameplay.Characters.Common;
using UnityEngine;

namespace game.Gameplay.Characters.Player
{
	public class PlayerCharacterContext {
		public Transform transform { get; }
		public Healthable healthable { get; }
		
		public CharacterMovement movement { get; }
		public CharacterEquipmentManger equipmentManger { get; }
		public ICharacterAnimation<CharacterAnimationSet, CharacterAnimationEnum, AnimationClip> animation { get; }
		public PlayerCharacterData data { get; }
		public BoneListenerManager boneListenerManager { get; }
		public Transform target { get; }

		public CharacterStateMachine<CharacterStateEnum, PlayerCharacterContext> mainStateMachine { get; set; }
		public CharacterStateMachine<PlayerActionStateEnum, PlayerCharacterContext> actionStateMachine { get; set; }

		public Camera camera;
		public readonly ICharacter character;

		public PlayerCharacterContext(Healthable healthable, CharacterMovement movement, CharacterAnimation animation, PlayerCharacterData data,
			Transform transform, CharacterEquipmentManger equipmentManger, BoneListenerManager boneListenerManager, ICharacter character, Transform target)
		{
			this.character = character;
			this.data = data;
			this.healthable = healthable;
			this.movement = movement;
			this.animation = animation;
			this.transform = transform;
			this.equipmentManger = equipmentManger;
			this.boneListenerManager = boneListenerManager;
			this.target = target;
		}
	}
}