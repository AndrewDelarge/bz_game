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

            input.keyPressed += DispatchInputPressed;
            input.inputVector += DispatchInputVector;
            
            _inputs.Add(input);
        }
        
        
        public void RegisterControlable(IControlable control)
        {
            if (_controlables.Contains(control))
            {
                return;
            }
            
            _controlables.Add(control);
        }

        private void DispatchInputPressed(KeyCode key)
        {
            foreach (var controlable in _controlables)
            {
                controlable.OnInputKeyPressed(key);
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