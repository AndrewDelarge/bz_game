using System;
using Core.InputSystem.Interfaces;
using game.core.InputSystem;
using UnityEngine;

namespace game.gameplay.control
{
    public class Input : MonoBehaviour, IInputable
    {
        public event Action<KeyCode> keyDown;
        public event Action<KeyCode> keyPressed;
        public event Action<Vector3> inputVector;
        
        private KeyCode[] trackingKeys = new[]
        {
            KeyCode.W,
            KeyCode.A,
            KeyCode.D,
            KeyCode.S
        };

        private void Start()
        {
            Core.Get<InputManager>().RegisterInput(this);
        }

        private void FixedUpdate()
        {
            if (UnityEngine.Input.GetAxis("Horizontal") != 0 || UnityEngine.Input.GetAxis("Vertical") != 0)
            {
                inputVector?.Invoke(new Vector3(UnityEngine.Input.GetAxis("Horizontal"), 0, UnityEngine.Input.GetAxis("Vertical")));
            }
            
            for (int i = 0; i < trackingKeys.Length; i++)
            {
                if (UnityEngine.Input.GetKey(trackingKeys[i]))
                {
                    keyPressed?.Invoke(trackingKeys[i]);
                }
            }
        }
    }
}