using core.InputSystem.Interfaces;
using UnityEngine;

namespace game.Source.Gameplay.Characters
{
    public partial class Character
    {
        public abstract class PlayerStateBase : CharacterState, IControlable
        {
            public PlayerStateBase(Character character) : base(character) { }

            protected Character character => (Character) base.character;

            public override void Enter()
            {
                Debug.Log($"Enter {GetType()}");
            }

            public override void Exit()
            {
                Debug.Log($"Exit {GetType()}");
            }
            
            public virtual void OnVectorInput(Vector3 vector3) { }

            public virtual void OnInputKeyPressed(KeyCode keyCode) { }

            public virtual void OnInputKeyDown(KeyCode keyCode) { }

            public virtual void OnInputKeyUp(KeyCode keyCode) { }
        }
    }
}