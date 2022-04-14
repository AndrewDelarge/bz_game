using game.core.InputSystem;
using UnityEngine;

namespace game.Source.Gameplay.Characters
{
    public abstract class CharacterState : BaseState
    {
        protected ICharacter character;
        
        public CharacterState(ICharacter character)
        {
            this.character = character;
        }
    }

    public abstract class BaseState
    {
        public abstract void Enter();

        public abstract void Exit();

        public abstract void HandleState();
        
        public abstract void HandleInput(InputData data);
    }
}