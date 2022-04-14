﻿using game.core.InputSystem;
using game.core.storage;
using game.Source.core.Common.Helpers;
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
                _impulsTime = _endTime - character.data.kickPhysicsImpulseDelay;
                character._animation.PlayAnimation(character.animationSet.testClip);
            }
            
            private void ProduceKickImpulse()
            {
                var distance  = character.data.kickFlightSphereDistance ;
                var radius    = character.data.kickSphereRadius   ;
                var viewAngle = character.data.kickAngle;
                var kickPower = character.data.kickPower;
                
                var centerPos = character._movement.transform.position;
                RaycastHit[] raycastHits = new RaycastHit[15];
                Physics.SphereCastNonAlloc(centerPos, radius, Vector3.forward, raycastHits, distance, (int) GameLayers.PHYSICS_OBJECTS);
                
                for (int i = 0; i < raycastHits.Length; i++)
                {
                    var rigidbody  = raycastHits[i].rigidbody;

                    if (rigidbody != null && VectorHelper.IsInViewAngle(character._movement.transform, rigidbody.transform.position, viewAngle))
                    {
                        rigidbody.AddForce(VectorHelper.GetDirection(centerPos, rigidbody.transform.position).normalized * kickPower, ForceMode.Impulse);
                    } 
                }
            }
        }
    }
}