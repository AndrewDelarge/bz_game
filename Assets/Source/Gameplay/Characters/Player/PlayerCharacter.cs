using System;
using System.Collections.Generic;
using core.InputSystem.Interfaces;
using game.core.InputSystem;
using game.core.Storage.Data.Character;
using game.gameplay.characters;
using game.Source.Gameplay.Characters.AI;
using game.Source.Gameplay.Characters.Common;
using UnityEngine;

namespace game.Source.Gameplay.Characters.Player
{
    public class PlayerCharacter : MonoBehaviour, IControlable, ICharacter
    {
        [Header("Components")]
        [SerializeField] private CharacterMovement _movement;
        [SerializeField] private CharacterAnimation _animation;
        [SerializeField] private Healthable _healthable;
        // TODO: Camera manager and controller
        [SerializeField] private Camera _camera;


        [SerializeField] private CharacterAnimData animationSet;
        [SerializeField] private PlayerCharacterCommonData data;

        private BaseStateMachine _mainStateMachine = new BaseStateMachine();
        private BaseStateMachine _actionStateMachine = new BaseStateMachine();

        private InputData _data;


        private Dictionary<Type, CharacterState> _states = new() {
            {typeof(PlayerIdleState), new PlayerIdleState()},
            {typeof(PlayerMoveState), new PlayerMoveState()},
        };
        
        private Dictionary<Type, CharacterState> _actionStates = new() {
            {typeof(PlayerActionIdleState), new PlayerActionIdleState()},
            {typeof(PlayerKickState), new PlayerKickState()},
        };
        
        public bool isPlayer => true;
        public bool isListen => true;

        public void Init()
        {
            game.Core.Get<IInputManager>().RegisterControlable(this);

            _animation.Init(animationSet);
            
            InitStates();
        }

        private void Update()
        {
            if (_data != null)
            {
                _actionStateMachine.currentState.HandleInput(_data);
                _mainStateMachine.currentState.HandleInput(_data);

                _data = null;
            }
            
            _actionStateMachine.currentState.HandleState();
            _mainStateMachine.currentState.HandleState();
        }

        private void InitStates() {
            var context = new PlayerCharacterContext(_healthable, _movement, _animation, animationSet, data, _movement.transform, _actionStateMachine, _mainStateMachine, _states , _actionStates);
            context.camera = _camera;

            foreach (var state in _states.Values) {
                state.Init(context);
            }
            
            foreach (var state in _actionStates.Values) {
                state.Init(context);
            }
            
            _mainStateMachine.ChangeState(_states[typeof(PlayerIdleState)]);
            _actionStateMachine.ChangeState(_actionStates[typeof(PlayerActionIdleState)]);
        }
        

        public void OnDataUpdate(InputData data)
        {
            _data = data;
        }
    }
}