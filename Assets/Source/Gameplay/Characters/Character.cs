using System.Collections.Generic;
using core.InputSystem.Interfaces;
using game.core.InputSystem;
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
        
        
        private CharacterStateMachine _stateMachine = new CharacterStateMachine();
        
        private CharacterState _idleState;
        private CharacterMoveState _moveState;
        private Queue<Vector3> _moves = new Queue<Vector3>();

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
            game.Core.Get<IInputManager>().RegisterControlable(_moveState);

        }
        
        public void OnVectorInput(Vector3 vector3)
        {
            _moves.Enqueue(vector3);
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