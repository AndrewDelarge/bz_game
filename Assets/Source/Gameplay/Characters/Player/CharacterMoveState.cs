using core.InputSystem.Interfaces;
using game.core.InputSystem;
using UnityEngine;

namespace game.Source.Gameplay.Characters
{
    public partial class Character
    {
        public class CharacterMoveState : PlayerStateBase
        {
            public CharacterMoveState(Character character) : base(character) {}

            private bool _sprint;
            private Vector2 _move;
            private float _currentSprintMultiplier = 1f;
            
            public override void HandleState()
            {
                var direction = _move.normalized;
                var angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + character._camera.transform.eulerAngles.y;
                var moveDirection = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;

                var characterSpeed = character.normalSpeed * GetSpeedMultiplier();
                
                var move = new CharacterMove(moveDirection, characterSpeed, angle);
                
                character._movement.Move(move);
                
                character._animation.SetMotionVelocityPercent(character._movement.GetHorizontalVelocity() /
                                                              (character.normalSpeed * character.speedMultiplier));
            }

            public override void HandleInput(InputData data)
            {
                _move = data.move.isAbsorbed ? Vector2.zero : data.move.value;
                
                if (_move == Vector2.zero)
                {
                    character._stateMachine.ChangeState(character._idleState);
                    return;
                }
                
                var sprint = data.GetAction(InputActionType.SPRINT);
                
                _sprint = false;
                
                if (sprint != null && sprint.value.status == InputStatus.PRESSED)
                {
                    _sprint = true;
                }
                
                var kick = data.GetAction(InputActionType.KICK);
                
                if (kick != null && kick.value.status == InputStatus.DOWN && _sprint == false)
                {
                    kick.isAbsorbed = true;
                    character._actionStateMachine.ChangeState(character._kickActionState);
                }
            }

            private float GetSpeedMultiplier()
            {
                if (!_sprint)
                {
                    _currentSprintMultiplier = 1;
                }
                else
                {
                    _currentSprintMultiplier = Mathf.Lerp(_currentSprintMultiplier, character.speedMultiplier,
                        character.speedSmoothTime * Time.deltaTime);
                }

                return _currentSprintMultiplier;
            }
        }
    }
}