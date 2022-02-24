using UnityEngine;

namespace game.Source.Gameplay.Characters
{
    public partial class Character
    {
        public class CharacterMoveState : PlayerStateBase
        {
            public CharacterMoveState(Character character) : base(character) {}

            public override void HandleState()
            {
                if (character._moves.Count == 0)
                {
                    character._stateMachine.ChangeState(character._idleState);
                    return;
                }
                
                var move = character._moves.Dequeue();
                
                var direction = move.vector.normalized;
                var angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + character._camera.transform.eulerAngles.y;
                var moveDirection = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
                var speed = 1f;
                var multiplier = 3f;
            
                move.Update(moveDirection, multiplier, angle);
            
                character._movement.Move(move);
                character._animation.SetMotionVelocityPercent(character._movement.GetHorizontalVelocity() / speed * multiplier);
            }
        }
    }
}