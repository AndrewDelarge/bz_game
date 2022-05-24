using System;
using System.Collections.Generic;
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
        private Transform _transform;
        private Healthable _healthable;
        private BaseStateMachine _mainStateMachine;
        protected CharacterCommonData _data;
        protected Dictionary<Type, CharacterState> _states;

        public ICharacterMovement movement => _movement;
        public ICharacterAnimation animation => _animation;
        public CharacterAnimData animationSet => _animationData;
        public Transform transform => _transform;
        public BaseStateMachine mainStateMachine => _mainStateMachine;
        public CharacterCommonData data => _data;
        public Healthable healthable => _healthable;
        public IReadOnlyDictionary<Type, CharacterState> states => _states;

        public CharacterContext(Healthable healthable, CharacterMovement movement, CharacterAnimation animation, CharacterAnimData animationData, Transform transform, CharacterCommonData data, BaseStateMachine mainStateMachine, Dictionary<Type, CharacterState> states) {
            _healthable = healthable;
            _movement = movement;
            _animation = animation;
            _animationData = animationData;
            _transform = transform;
            _mainStateMachine = mainStateMachine;
            _data = data;
            _states = states;
        }
    }
}