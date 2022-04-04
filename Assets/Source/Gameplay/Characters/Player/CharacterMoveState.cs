using core.InputSystem.Interfaces;
using UnityEngine;

namespace game.Source.Gameplay.Characters
{
    public partial class Character
    {
        public class CharacterMoveState : PlayerStateBase
        {
            public CharacterMoveState(Character character) : base(character) {}

            private bool _sprint;
            private float _currentSprintMultiplier = 1f;
            
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

                CalculateSpeedMultiplier();
                
                var characterSpeed = character.normalSpeed * _currentSprintMultiplier;
                
                var move = new CharacterMove(moveDirection, characterSpeed, angle);
                
                character._movement.Move(move);
                
                character._animation.SetMotionVelocityPercent(character._movement.GetHorizontalVelocity() /
                                                              (character.normalSpeed * character.speedMultiplier));
            }
            
            public override void OnInputKeyDown(KeyCode keyCode)
            {
                if (keyCode == KeyCode.LeftShift)
                {
                    _sprint = true;
                }
            }

            public override void OnInputKeyUp(KeyCode keyCode)
            {
                if (keyCode == KeyCode.LeftShift)
                {
                    _sprint = false;
                }
            }

            private void CalculateSpeedMultiplier()
            {
                if (! _sprint)
                {
                    _currentSprintMultiplier = 1;
                    return;
                }

                _currentSprintMultiplier = Mathf.Lerp(_currentSprintMultiplier, character.speedMultiplier,
                    character.speedSmoothTime * Time.deltaTime);
            }
        }
    }
}