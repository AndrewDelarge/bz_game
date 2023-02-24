using game.core.Storage.Data.Character;
using game.Gameplay.Characters.Common;
using UnityEngine;

namespace game.Gameplay.Characters
{
    public class CharacterContext
    {
        private CharacterMovement _movement;
        private CharacterAnimation _animation;
        private CharacterAnimationSet _characterAnimationSet;
        private Transform _transform;
        private Healthable _healthable;
        private CharacterStateMachine<CharacterStateEnum> _mainStateMachine;
        protected CharacterCommonData _data;

        public ICharacterMovement movement => _movement;
        public ICharacterAnimation animation => _animation;
        public CharacterAnimationSet characterAnimationSet => _characterAnimationSet;
        public Transform transform => _transform;
        public CharacterStateMachine<CharacterStateEnum> mainStateMachine => _mainStateMachine;
        public CharacterCommonData data => _data;
        public Healthable healthable => _healthable;

        public CharacterContext(Healthable healthable, CharacterMovement movement, CharacterAnimation animation, 
            CharacterAnimationSet characterAnimationSet, Transform transform, CharacterCommonData data, 
            CharacterStateMachine<CharacterStateEnum> mainStateMachine) {
            _healthable = healthable;
            _movement = movement;
            _animation = animation;
            _characterAnimationSet = characterAnimationSet;
            _transform = transform;
            _mainStateMachine = mainStateMachine;
            _data = data;
        }
    }
}