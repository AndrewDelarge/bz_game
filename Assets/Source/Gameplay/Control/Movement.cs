using core.InputSystem.Interfaces;
using game.core.InputSystem;
using game.Source.Gameplay.Characters;
using UnityEngine;

namespace game.gameplay.control
{
    public class Movement : MonoBehaviour, IControlable
    {
        [SerializeField] private float _speed = 1.5f;
        [SerializeField] private float _sprintSpeedMultiplier = 2f;

        [SerializeField] private CharacterController _characterController;
        [SerializeField] private CharacterAnimation _characterAnimator;
        
        public bool isListen { get; }
        
        private Camera _mainCamera;
        private Vector3 _horizontalVelocity;
        private Vector3 _lastPosition;
        private float _rotationVelocity;
        private bool _sprint;


        private void Start()
        {
            Core.Get<IInputManager>().RegisterControlable(this);
            
            //TODO remake to player camera
            _mainCamera = Camera.main;
            
            _lastPosition = transform.position;
        }

        private void Update()
        {
            UpdateHorizontalVelocity();
            ApplyGravity();

            _characterAnimator.SetMotionVelocityPercent(_horizontalVelocity.magnitude / (_speed * _sprintSpeedMultiplier));
        }

        public void OnVectorInput(Vector3 vector3)
        {
            var direction = vector3.normalized;
            var angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
            var rotationAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, ref _rotationVelocity, .05f);
            var moveDirection = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
            var speedMultiplier = _speed;
            
            if (_sprint)
                speedMultiplier *= _sprintSpeedMultiplier;
            
            transform.rotation = Quaternion.Euler(0f, rotationAngle, 0f);

            _characterController.Move(moveDirection.normalized * speedMultiplier * Time.deltaTime);
        }
        
        public void OnInputKeyDown(KeyCode keyCode)
        {
            if (keyCode == KeyCode.LeftShift) 
                _sprint = true;
        }

        public void OnInputKeyUp(KeyCode keyCode)
        {
            if (keyCode == KeyCode.LeftShift) 
                _sprint = false;
        }

        public void OnDataUpdate(InputData data)
        {
            
        }

        public void OnInputKeyPressed(KeyCode keyCode) {}

        private void UpdateHorizontalVelocity()
        {
            Vector3 vel = (transform.position - _lastPosition) / Time.deltaTime;
            _lastPosition = transform.position;
            _horizontalVelocity = new Vector3(vel.x, 0, vel.z);
        }

        private void ApplyGravity()
        {
            var direction = Vector3.zero;
            direction.y = _characterController.isGrounded ? -.05f : -9.8f;
            _characterController.Move(direction * Time.deltaTime);
        }
    }
}