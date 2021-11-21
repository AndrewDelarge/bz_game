﻿using System;
using UnityEngine;

namespace Core.InputSystem.Interfaces
{
    public interface IInputable
    {
        public event Action<KeyCode> keyDown;
        public event Action<KeyCode> keyUp;

        public event Action<Vector3> inputVector;
    }
}