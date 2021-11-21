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
        public event Action<KeyCode> keyUp;
        public event Action<Vector3> inputVector;
        
        private KeyCode[] _trackingKeys = {
            KeyCode.W,
            KeyCode.A,
            KeyCode.D,
            KeyCode.S,
            KeyCode.LeftShift
        };

        private void Start()
        {
            Core.Get<InputManager>().RegisterInput(this);
        }

        private void Update()
        {
            if (UnityEngine.Input.GetAxis("Horizontal") != 0 || UnityEngine.Input.GetAxis("Vertical") != 0)
            {
                inputVector?.Invoke(new Vector3(UnityEngine.Input.GetAxis("Horizontal"), 0, UnityEngine.Input.GetAxis("Vertical")));
            }
            
            for (int i = 0; i < _trackingKeys.Length; i++)
            {
                if (UnityEngine.Input.GetKeyDown(_trackingKeys[i]))
                {
                    keyDown?.Invoke(_trackingKeys[i]);
                }
                if (UnityEngine.Input.GetKeyUp(_trackingKeys[i]))
                {
                    keyUp?.Invoke(_trackingKeys[i]);
                }
            }
            
        }
    }
}