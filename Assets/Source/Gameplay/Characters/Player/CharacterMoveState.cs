using core.InputSystem.Interfaces;
using UnityEngine;

namespace game.Source.Gameplay.Characters
{
    public partial class Character
    {
        public class CharacterMoveState : PlayerStateBase, IControlable
        {
            public CharacterMoveState(Character character) : base(character) {}

            private bool _sprint;
            
            public override void HandleState()
            {
                if (character._moves.Count == 0)
                {
                    character._stateMachine.ChangeState(character._idleState);
                    return;
                }
                
                var moveVector3 = character._moves.Dequeue();
                
                var direction = moveVector3.normalized;
                var angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + character._camera.transform.eulerAngles.y;
                var moveDirection = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;

                var characterSpeed = character.normalSpeed * (_sprint ? character.speedMultiplier : 1);
                
                var move = new CharacterMove(moveDirection, characterSpeed, angle);
                
                character._movement.Move(move);
                
                character._animation.SetMotionVelocityPercent(character._movement.GetHorizontalVelocity() /
                                                              (character.normalSpeed * character.speedMultiplier));
            }

            public void OnVectorInput(Vector3 vector3) { }

            public void OnInputKeyPressed(KeyCode keyCode) { }

            public void OnInputKeyDown(KeyCode keyCode)
            {
                if (keyCode == KeyCode.LeftShift)
                {
                    _sprint = true;
                }
            }

            public void OnInputKeyUp(KeyCode keyCode)
            {
                if (keyCode == KeyCode.LeftShift)
                {
                    _sprint = false;
                }
            }
        }
    }
}