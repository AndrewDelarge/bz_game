using game.core.Storage.Data.Character;
using game.Gameplay.Characters.Common;
using UnityEngine;

namespace game.Gameplay.Characters.Player
{
	public class PlayerCharacterContext {
		private CharacterStateMachine<PlayerActionStateEnum, PlayerCharacterContext> _actionStateMachine;
		private CharacterMovement _movement;
		private CharacterAnimation _animation;
		private CharacterAnimationSet _characterAnimationSet;
		private Transform _transform;
		private Healthable _healthable;
		private CharacterStateMachine<CharacterStateEnum, PlayerCharacterContext> _mainStateMachine;
		private PlayerCharacterCommonData _data;

		public Camera camera;
		
		public ICharacterMovement movement => _movement;
		public ICharacterAnimation animation => _animation;
		public CharacterAnimationSet characterAnimationSet => _characterAnimationSet;
		public Transform transform => _transform;
		public Healthable healthable => _healthable;
		public PlayerCharacterCommonData data => _data;
		public CharacterStateMachine<PlayerActionStateEnum, PlayerCharacterContext> actionStateMachine {
			get => _actionStateMachine;
			set => _actionStateMachine = value;
		}
		
		public CharacterStateMachine<CharacterStateEnum, PlayerCharacterContext> mainStateMachine {
			get => _mainStateMachine;
			set => _mainStateMachine = value;
		}

		public PlayerCharacterContext(Healthable healthable, CharacterMovement movement, CharacterAnimation animation, 
			CharacterAnimationSet characterAnimationSet, PlayerCharacterCommonData data, Transform transform) {
			_data = data;
			_healthable = healthable;
			_movement = movement;
			_animation = animation;
			_characterAnimationSet = characterAnimationSet;
			_transform = transform;
		}
	}
}