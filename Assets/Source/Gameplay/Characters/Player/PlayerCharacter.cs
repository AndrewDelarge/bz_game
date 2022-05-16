using core.InputSystem.Interfaces;
using game.core.InputSystem;
using game.core.Storage.Data.Character;
using game.gameplay.characters;
using game.Source.Gameplay.Characters.Common;
using UnityEngine;

namespace game.Source.Gameplay.Characters.Player
{
    public class PlayerCharacter : MonoBehaviour, IControlable, ICharacter
    {
        [Header("Components")]
        [SerializeField] private CharacterMovement _movement;
        [SerializeField] private CharacterAnimation _animation;
        [SerializeField] private Camera _camera;


        [SerializeField] private CharacterAnimData animationSet;
        [SerializeField] private PlayerCommonData data;
        public bool isListen => true;

        private BaseStateMachine _mainStateMachine = new BaseStateMachine();
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
                _mainStateMachine.currentState.HandleInput(_data);
            }
            
            _actionStateMachine.currentState.HandleState();
            _mainStateMachine.currentState.HandleState();
        }

        private void InitStates() {
            var context = new PlayerCharacterContext(_movement, _animation, animationSet, data, _movement.transform, _actionStateMachine, _mainStateMachine);
            context.camera = _camera;
            

            _idleState = new PlayerIdleState(context);
            _moveState = new PlayerMoveState(context);
            
            _idleActionState = new PlayerActionIdleState(context);
            _kickActionState = new PlayerKickState(context);
            
            context.idleState = _idleState;
            context.moveState = _moveState;
            context.idleActionState = _idleActionState;
            context.kickActionState = _kickActionState;
            
            _mainStateMachine.ChangeState(_idleState);
            _actionStateMachine.ChangeState(_idleActionState);
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