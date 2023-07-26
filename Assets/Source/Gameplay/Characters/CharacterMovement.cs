﻿using System.Collections.Generic;
using game.Gameplay.Characters.Common;
using UnityEngine;

namespace game.Gameplay.Characters
{
    public class CharacterMovement : MonoBehaviour, ICharacterMovement
    {
        private const float ON_GROUND_GRAVITY = -.05f;

        // TODO Use own movement realization
        [SerializeField] private CharacterController _characterController;
        
        public Vector3 horizontalVelocity { get; private set; }

        private float _rotationVelocity;

        private bool _sprint;
        private Queue<CharacterMove> _movesQueue = new ();
        private bool _rotationLocked;

        private void Update()
        {
            UpdateHorizontalVelocity();
            
            DoMove();
        }

        public void Disable() {
            _characterController.enabled = false;
            enabled = false;
        }

        public void SetLockRotation(bool value) {
            _rotationLocked = value;
        }

        public void Move(CharacterMove move)
        {
            _movesQueue.Enqueue(move);
        }

        public void Rotate(float angle)
        {
            DoRotate(angle);
        }

        public float GetHorizontalVelocity()
        {
            return horizontalVelocity.magnitude;
        }

        public void ForceStop() {
            horizontalVelocity = Vector3.zero;
            _movesQueue.Clear();
        }

        private void DoMove()
        {
            var moveDirection = Vector3.zero;
            var multiplier = 1f;
            
            if (_movesQueue.Count != 0)
            {
                var move = _movesQueue.Dequeue();
                
                moveDirection = move.vector;
                multiplier = move.multiplier;

                if (_rotationLocked == false) {
                    DoRotate(move.angle);
                }
            }

            moveDirection.y = _characterController.isGrounded ? ON_GROUND_GRAVITY : Physics.gravity.y;
            
            _characterController.Move(moveDirection.normalized * multiplier * Time.deltaTime);
        }

        private void DoRotate(float angle)
        {
            var rotationAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, ref _rotationVelocity, .05f);
            transform.rotation = Quaternion.Euler(0f, rotationAngle, 0f);
        }

        private void UpdateHorizontalVelocity()
        {
            var vel = _characterController.velocity;
            horizontalVelocity = new Vector3(vel.x, 0, vel.z);
        }
    }
    
    

    
}