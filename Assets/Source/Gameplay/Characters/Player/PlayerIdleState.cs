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
                if (character._moves.Count != 0)
                {
                    character._stateMachine.ChangeState(character._moveState);
                    return;
                }
                
                character._animation.SetMotionVelocityPercent(character._movement.GetHorizontalVelocity() /
                                                              (character.normalSpeed * character.speedMultiplier));
            }

            public override void HandleInput(InputData data)
            {
                var kick = data.GetAction(InputActionType.KICK);
                if (kick != null && kick.value.status == InputStatus.UP)
                {
                    kick.isAbsorbed = true;
                    character._animation.PlayAnimation(character.animationSet.testClip);
                }
            }

            public override void OnInputKeyDown(KeyCode keyCode)
            {
                base.OnInputKeyDown(keyCode);
                
                // TODO KEYMAP OR ACTIONS
                if (keyCode == KeyCode.F)
                {
                    character._animation.PlayAnimation(character.animationSet.testClip);
                }
            }
        }
    }
}