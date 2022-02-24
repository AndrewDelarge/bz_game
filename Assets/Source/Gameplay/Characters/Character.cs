using System.Collections.Generic;
using core.InputSystem.Interfaces;
using game.core.InputSystem;
using game.gameplay.characters;
using UnityEngine;

namespace game.Source.Gameplay.Characters
{
    public partial class Character : MonoBehaviour, IControlable, ICharacter
    {
        private CharacterStateMachine _stateMachine = new CharacterStateMachine();
        
        private CharacterState _idleState;
        private CharacterState _moveState;
        private CharacterMove _move { get; } = new CharacterMove(Vector3.zero, 0f, 0f);
        private Queue<CharacterMove> _moves = new Queue<CharacterMove>();


        [Header("Components")]
        [SerializeField] private CharacterMovement _movement;
        [SerializeField] private CharacterAnimation _animation;
        [SerializeField] private Camera _camera;

        public void Init()
        {
            game.Core.Get<IInputManager>().RegisterControlable(this);

            InitStates();
            _stateMachine.ChangeState(_idleState);
        }

        private void Update()
        {
            _stateMachine.currentState.HandleState();
        }

        private void InitStates()
        {
            _idleState = new PlayerIdleState(this);
            _moveState = new CharacterMoveState(this);
        }
        
        public void OnVectorInput(Vector3 vector3)
        {
            _moves.Enqueue(new CharacterMove(vector3, 0, 0));
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