using System.Collections.Generic;
using Core.InputSystem.Interfaces;
using game.core.InputSystem;
using UnityEngine;

namespace game.gameplay.control
{
    public class Control : MonoBehaviour, IControlable
    {
        [SerializeField] private float speedMultiplier = 3f;
        [SerializeField] private float angularDrag = .5f;
        
        private const float SPEED_MULTIPLIER = 3f;
        
        [SerializeField] private Rigidbody target;
        private Camera mainCamera;

        private Dictionary<KeyCode, Vector3> keyToDirectionDict = new Dictionary<KeyCode, Vector3>()
        {
            {KeyCode.A, Vector3.left},
            {KeyCode.W, Vector3.forward},
            {KeyCode.S, Vector3.back},
            {KeyCode.D, Vector3.right},
        };

        private void Start()
        {
            Core.Get<InputManager>().RegisterControlable(this);
            mainCamera = Camera.main;
            target.mass = 0.1f;
        }


        public void OnInputKeyPressed(KeyCode keyCode)
        {
        }

        public void OnVectorInput(Vector3 vector3)
        {
            var goPosition = mainCamera.transform.TransformDirection(vector3);
            goPosition.y = 0;

            target.velocity = goPosition.normalized * speedMultiplier;
            
            Debug.Log($"final {goPosition} v3 {vector3} go norm norm {goPosition.normalized}");
        }

        public void OnInputKeyDown(KeyCode keyCode)
        {
        }
    }
}