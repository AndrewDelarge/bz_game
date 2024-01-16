using System;
using game.core.Common;
using UnityEngine;

namespace game.core.InputSystem.Interfaces
{
    public interface IInputable
    {
        public event Action<KeyCode> keyDown;
        public event Action<KeyCode> keyUp;

        public event Action<Vector3> inputVector;

        public IWhistle<InputData> updated { get; }

        public void Dispose();
    }
}