using game.core.Storage.Data.Character;
using game.Gameplay.Characters.Common;
using UnityEngine;

namespace game.Gameplay.Characters.Player
{
	public class PlayerCharacterContext {
		private Transform _transform;
		private Healthable _healthable;
		
		private CharacterMovement _movement;
		private CharacterEquipmentManger _equipmentManger;
		private CharacterAnimation _animation;
		private PlayerCharacterData _data;
		private BoneListenerManager _boneListenerManager;

		private CharacterStateMachine<CharacterStateEnum, PlayerCharacterContext> _mainStateMachine;
		private CharacterStateMachine<PlayerActionStateEnum, PlayerCharacterContext> _actionStateMachine;

		public Camera camera;
		public readonly ICharacter character;
		public Transform transform => _transform;
		public Healthable healthable => _healthable;
		public PlayerCharacterData data => _data;
		public BoneListenerManager boneListenerManager => _boneListenerManager;

		public CharacterEquipmentManger equipmentManger => _equipmentManger;
		public ICharacterMovement movement => _movement;
		public ICharacterAnimation<CharacterAnimationSet, CharacterAnimationEnum, AnimationClip> animation => _animation;

		public CharacterStateMachine<PlayerActionStateEnum, PlayerCharacterContext> actionStateMachine {
			get => _actionStateMachine;
			set => _actionStateMachine = value;
		}
		
		public CharacterStateMachine<CharacterStateEnum, PlayerCharacterContext> mainStateMachine {
			get => _mainStateMachine;
			set => _mainStateMachine = value;
		}

		public PlayerCharacterContext(Healthable healthable, CharacterMovement movement, CharacterAnimation animation, PlayerCharacterData data, 
			Transform transform, CharacterEquipmentManger equipmentManger, BoneListenerManager boneListenerManager, ICharacter character)
		{
			this.character = character;
			_data = data;
			_healthable = healthable;
			_movement = movement;
			_animation = animation;
			_transform = transform;
			_equipmentManger = equipmentManger;
			_boneListenerManager = boneListenerManager;
		}
	}
}