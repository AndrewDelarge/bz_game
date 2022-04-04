using System;
using System.Collections.Generic;
using core.InputSystem.Interfaces;
using game.core.common;
using UnityEngine;

namespace game.core.InputSystem
{
    public interface IInputManager
    {
        void RegisterInput(IInputable input);
        void RegisterControlable(IControlable control);
    }

    public class InputManager : ICoreManager, IInputManager
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

        public void RegisterControlable(IControlable control)
        {
            if (_controlables.Contains(control))
            {
                return;
            }
            
            _controlables.Add(control);
        }
        
        private void DispatchInputKeyDown(KeyCode key)
        {
            foreach (var controlable in _controlables)
            {
                if (controlable.isListen == false) 
                    continue;
                
                controlable.OnInputKeyDown(key);
            }
        }
        
        private void DispatchInputKeyUp(KeyCode key)
        {
            foreach (var controlable in _controlables)
            {
                if (controlable.isListen == false) 
                    continue;
                
                controlable.OnInputKeyUp(key);
            }
        }

        private void DispatchInputVector(Vector3 vector)
        {
            foreach (var controlable in _controlables)
            {
                if (controlable.isListen == false) 
                    continue;
                
                controlable.OnVectorInput(vector);
            }
            
        }

    }
}