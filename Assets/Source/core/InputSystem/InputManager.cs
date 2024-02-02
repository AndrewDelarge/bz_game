using System.Collections.Generic;
using game.core.InputSystem.Interfaces;
using game.core.common;

namespace game.core.InputSystem
{
    public interface IInputManager
    {
        void RegisterInput(IInputable input);
        void RegisterControlable(IControlable control);

        void RemoveControlable(IControlable control);
        void RemoveInput(IInputable input);
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
            foreach (var controlable in _controlables) {
                if (controlable.isListen == false) 
                    continue;
                
                controlable.OnDataUpdate(data);
            }
        }

        public void RegisterControlable(IControlable control)
        {
            if (_controlables.Contains(control)) {
                return;
            }
            
            _controlables.Add(control);
        }

        public void RemoveControlable(IControlable control) {
            if (_controlables.Contains(control) == false) {
                return;
            }
            
            _controlables.Remove(control);
        }

        public void RemoveInput(IInputable input) {
            if (_inputs.Contains(input) == false) {
                return;
            }

            input.Dispose();
            
            _inputs.Remove(input);
        }

        public void Reset() {
            foreach (var input in _inputs) {
                input.Dispose();
            }
            
            _controlables.Clear();
        }

    }
}