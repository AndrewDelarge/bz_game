using System.Collections.Generic;
using Core.InputSystem.Interfaces;
using game.core.common;
using UnityEngine;

namespace game.core.InputSystem
{
    public class InputManager : ICoreManager
    {
        private List<IInputable> _inputs = new List<IInputable>();
        private List<IControlable> _controlables = new List<IControlable>();
        
        public void RegisterInput(IInputable input)
        {
            if (_inputs.Contains(input))
            {
                return;
            }

            input.inputVector += DispatchInputVector;
            input.keyUp += DispatchInputKeyUp;
            input.keyDown += DispatchInputKeyDown;
            
            _inputs.Add(input);
        }

        private void DispatchInputKeyDown(KeyCode key)
        {
            foreach (var controlable in _controlables)
            {
                controlable.OnInputKeyDown(key);
            }
        }

        public void RegisterControlable(IControlable control)
        {
            if (_controlables.Contains(control))
            {
                return;
            }
            
            _controlables.Add(control);
        }

        private void DispatchInputKeyUp(KeyCode key)
        {
            foreach (var controlable in _controlables)
            {
                controlable.OnInputKeyUp(key);
            }
        }

        private void DispatchInputVector(Vector3 vector)
        {
            foreach (var controlable in _controlables)
            {
                controlable.OnVectorInput(vector);
            }
        }
        
    }
}