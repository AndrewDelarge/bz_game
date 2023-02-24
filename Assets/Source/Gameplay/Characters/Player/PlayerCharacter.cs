using System.Collections.Generic;
using game.core.InputSystem.Interfaces;
using game.core.InputSystem;
using game.core.Storage.Data.Character;
using game.gameplay.characters;
using game.Gameplay.Characters.Common;
using UnityEngine;

namespace game.Gameplay.Characters.Player
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

        private CharacterStateMachine<CharacterStateEnum> _mainStateMachine;
        private CharacterStateMachine<PlayerActionStateEnum> _actionStateMachine;

        private InputData _data;
        private bool isInited;

        public bool isPlayer => true;
        public bool isListen => true;

        public void Init()
        {
            AppCore.Get<IInputManager>().RegisterControlable(this);

            _animation.Init(animationSet);
            
            InitStates();

            isInited = true;
        }

        private void Update()
        {
            if (isInited == false) {
                return;
            }
            
            UpdateStateMachines();
        }

        private void UpdateStateMachines()
        {
            if (_data != null)
            {
                _actionStateMachine.HandleInput(_data);
                _mainStateMachine.HandleInput(_data);

                _data = null;
            }
            
            _actionStateMachine.HandleState();
            _mainStateMachine.HandleState();
        }

        private void InitStates()
        {
            _mainStateMachine = new CharacterStateMachine<CharacterStateEnum>(new Dictionary<CharacterStateEnum, CharacterState<CharacterStateEnum>>(){
                {CharacterStateEnum.IDLE, new PlayerIdleState()},
                {CharacterStateEnum.WALK, new PlayerWalkState()},
                {CharacterStateEnum.RUN, new PlayerRunState()},
            });

            _actionStateMachine = new CharacterStateMachine<PlayerActionStateEnum>(new Dictionary<PlayerActionStateEnum, CharacterState<PlayerActionStateEnum>>() {
                {PlayerActionStateEnum.IDLE, new PlayerActionWeaponEquip()},
                {PlayerActionStateEnum.KICK, new PlayerActionKickState()},
                // {PlayerActionStateEnum.EQUIP, new PlayerActionWeaponEquip()},
            });

            var context = new PlayerCharacterContext(_healthable, _movement, _animation, animationSet, data, 
                _movement.transform, _actionStateMachine, _mainStateMachine);
            
            context.camera = _camera;

            
            // TODO совсем избавится от контекста не получится мб хотябы не хранить ссылки на него в стейтах
            foreach (var state in _mainStateMachine.states.Values) {
                state.Init(context);
            }
            
            foreach (var state in _actionStateMachine.states.Values) {
                state.Init(context);
            }
            
            _mainStateMachine.ChangeState(CharacterStateEnum.IDLE);
            _actionStateMachine.ChangeState(PlayerActionStateEnum.IDLE);
            
            _mainStateMachine.onStateChanged.Add(_actionStateMachine.OnStateChangeHandler);
            _actionStateMachine.onStateChanged.Add(_mainStateMachine.OnStateChangeHandler);
            
            
            _actionStateMachine.ChangeState(PlayerActionStateEnum.EQUIP);
        }
        

        public void OnDataUpdate(InputData data)
        {
            _data = data;
        }
    }
}