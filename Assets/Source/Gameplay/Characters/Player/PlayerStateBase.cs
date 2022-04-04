using core.InputSystem.Interfaces;
using UnityEngine;

namespace game.Source.Gameplay.Characters
{
    public partial class Character
    {
        public abstract class PlayerStateBase : CharacterState, IControlable
        {
            public PlayerStateBase(Character character) : base(character) { }
            
            public bool isListen => _isListen;
            protected Character character => (Character) base.character;

            private bool _isListen;

            public override void Enter()
            {
                _isListen = true;
                Debug.Log($"Enter {GetType()}");
            }

            public override void Exit()
            {
                _isListen = false;
                Debug.Log($"Exit {GetType()}");
            }
            
            public virtual void OnVectorInput(Vector3 vector3) { }

            public virtual void OnInputKeyPressed(KeyCode keyCode) { }

            public virtual void OnInputKeyDown(KeyCode keyCode) { }

            public virtual void OnInputKeyUp(KeyCode keyCode) { }
        }
    }
}