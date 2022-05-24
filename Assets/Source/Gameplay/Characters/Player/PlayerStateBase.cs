using core.InputSystem.Interfaces;
using UnityEngine;

namespace game.Source.Gameplay.Characters
{

        public abstract class PlayerStateBase : CharacterState
        {
            protected PlayerCharacterContext context => (PlayerCharacterContext) base.context;

            public override void Enter()
            {
                Debug.Log($"Enter {GetType()}");
            }

            public override void Exit()
            {
                Debug.Log($"Exit {GetType()}");
            }
        }
    }
