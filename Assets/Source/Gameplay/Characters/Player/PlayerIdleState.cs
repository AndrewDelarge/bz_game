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
                                                              (character.normalSpeed * character.speedMultiplier));
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

        public class PlayerActionIdleState : PlayerStateBase
        {
            public PlayerActionIdleState(Character character) : base(character)
            {
            }

            public override void HandleState()
            {
            }

            public override void HandleInput(InputData data)
            {
            }
        }
        
        public class PlayerKickState : PlayerStateBase
        {
            private float _endTime = 0;
            
            public PlayerKickState(Character character) : base(character)
            {
            }

            public override void HandleState()
            {
                _endTime -= Time.deltaTime;
                
                if (_endTime <= 0)
                    character._actionStateMachine.ChangeState(character._idleActionState);
            }

            public override void HandleInput(InputData data)
            {
                data.move.isAbsorbed = true;
            }

            public override void Enter()
            {
                base.Enter();
                
                _endTime = character.animationSet.testClip.length * .9f;
                character._animation.PlayAnimation(character.animationSet.testClip);
            }
            
        }
    }
}