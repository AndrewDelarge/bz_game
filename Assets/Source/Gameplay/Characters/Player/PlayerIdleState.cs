using game.core.InputSystem;
using UnityEngine;

namespace game.Source.Gameplay.Characters
{
    public partial class Character
    {
        public class PlayerIdleState : PlayerStateBase
        {
            public PlayerIdleState(Character character) : base(character) {}

            public override void HandleState()
            {
                character._animation.SetMotionVelocityPercent(character._movement.GetHorizontalVelocity() /
                                                              (character.data.normalSpeed * character.data.speedMultiplier));
            }

            public override void HandleInput(InputData data)
            {
                var kick = data.GetAction(InputActionType.KICK);
                
                if (kick != null && kick.value.status == InputStatus.UP)
                {
                    kick.isAbsorbed = true;
                    character._actionStateMachine.ChangeState(character._kickActionState);
                }

                if (data.move.value != Vector2.zero && data.move.isAbsorbed == false)
                {
                    character._stateMachine.ChangeState(character._moveState);
                    return;
                }
            }
        }
    }
}