using System;
using System.Collections.Generic;
using Core.InputSystem.Interfaces;
using UnityEngine;

namespace Core.InputSystem
{
    public class Control : MonoBehaviour, IControlable
    {
        private const float SPEED_MULTIPLIER = 1f;
        
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
            mainCamera = Camera.current;
            InputManager.Instance.RegisterControlable(this);
        }


        public void OnInputKeyPressed(KeyCode keyCode)
        {
            if (keyToDirectionDict.ContainsKey(keyCode))
            {
                target.AddForce(keyToDirectionDict[keyCode] * SPEED_MULTIPLIER, ForceMode.Force);
            }
        }
    
        public void OnInputKeyDown(KeyCode keyCode)
        {
        
        }
    }
}