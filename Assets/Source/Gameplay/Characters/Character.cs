using System.Collections.Generic;
using core.InputSystem.Interfaces;
using game.core.InputSystem;
using game.core.Storage.Data.Character;
using game.gameplay.characters;
using UnityEngine;

namespace game.Source.Gameplay.Characters
{
    public partial class Character : MonoBehaviour, IControlable, ICharacter
    {
        [Header("Components")]
        [SerializeField] private CharacterMovement _movement;
        [SerializeField] private CharacterAnimation _animation;
        [SerializeField] private Camera _camera;

        // Model data
        [SerializeField] private float normalSpeed = 3f;
        [SerializeField] private float speedMultiplier = 1f;
        [SerializeField] private float speedSmoothTime = 1f;
        [SerializeField] private CharacterAnimData animationSet;
        public bool isListen => true;

        private BaseStateMachine _stateMachine = new BaseStateMachine();
        private BaseStateMachine _actionStateMachine = new BaseStateMachine();
        
        private PlayerStateBase _idleState;
        private PlayerStateBase _moveState;

        private PlayerStateBase _idleActionState;
        private PlayerStateBase _kickActionState;

        
        private InputData _data;


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
                _stateMachine.currentState.HandleInput(_data);
            }
            
            _actionStateMachine.currentState.HandleState();
            _stateMachine.currentState.HandleState();
        }

        private void InitStates()
        {
            _idleState = new PlayerIdleState(this);
            _moveState = new CharacterMoveState(this);
            
            _idleActionState = new PlayerActionIdleState(this);
            _kickActionState = new PlayerKickState(this);
            
            _stateMachine.ChangeState(_idleState);
            _actionStateMachine.ChangeState(_idleActionState);
            // game.Core.Get<IInputManager>().RegisterControlable(_moveState);
            // game.Core.Get<IInputManager>().RegisterControlable(_idleState);
        }
        


        public void OnDataUpdate(InputData data)
        {
            _data = data;
        }
        
        public void OnVectorInput(Vector3 vector3)
        {
        }

        public void OnInputKeyPressed(KeyCode keyCode)
        {
            
        }

        public void OnInputKeyDown(KeyCode keyCode)
        {
        }

        public void OnInputKeyUp(KeyCode keyCode)
        {
        }
    }
}