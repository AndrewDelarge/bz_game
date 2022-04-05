using System;
using game.core.InputSystem;
using UnityEngine;

namespace core.InputSystem.Interfaces
{
    public interface IInputable
    {
        public event Action<KeyCode> keyDown;
        public event Action<KeyCode> keyUp;

        public event Action<Vector3> inputVector;

        public event Action<InputData> updated;
    }
}