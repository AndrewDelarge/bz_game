using UnityEngine;

namespace game.Source.Gameplay.Characters
{
    public class Character
    {

        private Vector3 _move;
        private bool _sprint;
        private CharacterStateMachine _stateMachine;

        public void Init()
        {
            
        }
        
        public void HandleInput(Vector3 move)
        {
            
        }

        private void HandleInput(CharacterAction action, bool keyUp = false)
        {
            if (action == CharacterAction.RUN)
            {
                _sprint = true;
                
                if (keyUp)
                {
                    _sprint = false;
                }
            }
        }
    }

    public enum CharacterAction
    {
        RUN = 0,
    }

    public class CharacterStateMachine
    {
        public CharacterState currentState;
        public CharacterState action;

        public void ChangeState(CharacterState state)
        {
            
        }
    }

    public class CharacterState
    {

        public void handleState()
        {
            
        }
    }
}