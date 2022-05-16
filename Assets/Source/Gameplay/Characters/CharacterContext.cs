using game.core.Storage.Data.Character;
using game.gameplay.characters;
using game.Source.Gameplay.Characters.Common;
using UnityEngine;

namespace game.Source.Gameplay.Characters
{
    public class CharacterContext
    {
        private CharacterMovement _movement;
        private CharacterAnimation _animation;
        private CharacterAnimData _animationData;
        private PlayerCommonData _data;
        private Transform _transform;
        
        private BaseStateMachine _actionStateMachine;
        private BaseStateMachine _mainStateMachine;
        
        public ICharacterMovement movement => _movement;
        public ICharacterAnimation animation => _animation;
        public CharacterAnimData animationSet => _animationData;
        public PlayerCommonData data => _data;
        public Transform transform => _transform;
        
        public BaseStateMachine actionStateMachine => _actionStateMachine;
        public BaseStateMachine mainStateMachine => _mainStateMachine;

        public CharacterContext(CharacterMovement movement, CharacterAnimation animation, CharacterAnimData animationData, PlayerCommonData data, Transform transform, BaseStateMachine actionStateMachine, BaseStateMachine mainStateMachine) {
            _movement = movement;
            _animation = animation;
            _animationData = animationData;
            _data = data;
            _transform = transform;
            _actionStateMachine = actionStateMachine;
            _mainStateMachine = mainStateMachine;
        }
    }
}