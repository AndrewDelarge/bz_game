using game.core.InputSystem;
using UnityEngine;

namespace game.Source.Gameplay.Characters
{
    public partial class Character
    {
        public class PlayerKickState : PlayerStateBase
        {
            private float _endTime = 0;
            private float _impulsTime = 0;
            
            public PlayerKickState(Character character) : base(character)
            {
            }

            public override void HandleState()
            {
                _endTime -= Time.deltaTime;
                if (_endTime <= 0)
                    character._actionStateMachine.ChangeState(character._idleActionState);

                if (_endTime <= _impulsTime)
                {
                    ProduceKickImpulse();
                    
                    _impulsTime = int.MinValue;
                }
            }
            
            public override void HandleInput(InputData data)
            {
                data.move.isAbsorbed = true;
            }

            public override void Enter()
            {
                base.Enter();
                
                _endTime = character.animationSet.testClip.length * .9f;
                _impulsTime = _endTime - character.kickPhysicsImpulsDelay;
                character._animation.PlayAnimation(character.animationSet.testClip);
            }

            public bool IsInViewAngle(Transform current, Vector3 target, float viewAngle)
            {
                Vector3 dirToTarget = (target - current.position).normalized;

                if (Vector3.Angle(current.forward, dirToTarget) < viewAngle)
                    return true;

                return false;
            }

            protected Vector3 GetDirection(Vector3 from, Vector3 to)
            {
                Vector3 heading = to - from;
                float distance = heading.magnitude;
                return heading / distance;
            }
            
            private void ProduceKickImpulse()
            {
                var layerMask = 1 << 10;
                var distance  = character.distance ;
                var radius    = character.radius   ;
                var viewAngle = character.viewAngle;
                var kickPower = character.kickPower;
                
                var centerPos = character._movement.transform.position;
                RaycastHit[] raycastHits = new RaycastHit[15];
                Physics.SphereCastNonAlloc(centerPos, radius, Vector3.forward, raycastHits, distance, layerMask);
                
                for (int i = 0; i < raycastHits.Length; i++)
                {
                    var rigidbody  = raycastHits[i].rigidbody;

                    if (rigidbody != null && IsInViewAngle(character._movement.transform, rigidbody.transform.position, viewAngle))
                    {
                        rigidbody.AddForce(GetDirection(centerPos, rigidbody.transform.position) * kickPower, ForceMode.Impulse);
                    } 
                }
            }
        }
    }
}