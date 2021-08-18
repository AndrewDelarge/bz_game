using System;
using Core.InputSystem.Interfaces;
using UnityEngine;

namespace Core.InputSystem
{
    public class Input : MonoBehaviour, IInputable
    {
        public event Action<KeyCode> keyDown;
        public event Action<KeyCode> keyPressed;

        private KeyCode[] trackingKeys = new[]
        {
            KeyCode.W,
            KeyCode.A,
            KeyCode.D,
            KeyCode.S
        };

        private void Start()
        {
            InputManager.Instance.RegisterInput(this);
        }

        private void Update()
        {
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