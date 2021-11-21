﻿using System;
using System.Collections.Generic;
using Core.InputSystem.Interfaces;
using game.core.InputSystem;
using UnityEngine;

namespace game.gameplay.control
{
    public class Control : MonoBehaviour, IControlable
    {
        [SerializeField] private float _speed = 1.5f;
        [SerializeField] private float _sprintSpeedMultiplier = 2f;

        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Animator _characterAnimator;
        private Camera _mainCamera;

        private Vector3 _lastPosition;
        private float _rotationVelocity;
        private bool _sprint;
        private void Start()
        {
            Core.Get<InputManager>().RegisterControlable(this);
            
            //TODO remake to player camera
            _mainCamera = Camera.main;
            // target.mass = 0.1f;
        }


        public void OnInputKeyPressed(KeyCode keyCode)
        {
            
        }

        public void OnVectorInput(Vector3 vector3)
        {
            var direction = vector3.normalized;
            direction.y = _characterController.isGrounded ? -.05f : -9.8f;

            var angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
            var rotationAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, ref _rotationVelocity, .05f);
            var moveDirection = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
            var speedMultiplier = _speed;
            
            if (_sprint)
                speedMultiplier *= _sprintSpeedMultiplier;
            
            transform.rotation = Quaternion.Euler(0f, rotationAngle, 0f);

            _characterController.Move(moveDirection.normalized * speedMultiplier * Time.deltaTime);
        }

        private void FixedUpdate()
        {
            Vector3 vel = (transform.position - _lastPosition) / Time.deltaTime;
            _lastPosition = transform.position;
            
            
            //TODO MOVE to animator
            _characterAnimator.SetFloat("velocity", vel.magnitude / (_speed * _sprintSpeedMultiplier), .1f, Time.deltaTime);
        }

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