using System;
using System.Collections.Generic;
using game.core.InputSystem.Interfaces;
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

            input.updated.Add(DispatchInputDataUpdate);
            
            _inputs.Add(input);
        }

        private void DispatchInputDataUpdate(InputData data)
        {
            foreach (var controlable in _controlables)
            {
                if (controlable.isListen == false) 
                    continue;
                
                controlable.OnDataUpdate(data);
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

        public void Reset() {
            foreach (var input in _inputs) {
                input.Dispose();
            }
            
            _controlables.Clear();
        }

    }
}