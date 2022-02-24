using System;
using core.InputSystem.Interfaces;
using game.core.InputSystem;
using UnityEngine;

namespace game.gameplay.control
{
    public class InputListener : MonoBehaviour, IInputable
    {
        public event Action<KeyCode> keyDown;
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
            Core.Get<IInputManager>().RegisterInput(this);
        }

        private void Update()
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                inputVector?.Invoke(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
            }
            
            for (int i = 0; i < _trackingKeys.Length; i++)
            {
                if (Input.GetKeyDown(_trackingKeys[i]))
                {
                    keyDown?.Invoke(_trackingKeys[i]);
                }
                if (Input.GetKeyUp(_trackingKeys[i]))
                {
                    keyUp?.Invoke(_trackingKeys[i]);
                }
            }
            
        }
    }
}