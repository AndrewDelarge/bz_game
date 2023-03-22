using System.Collections.Generic;

namespace game.Gameplay.Common
{
    public class BaseStateMachineWithStack<T> : BaseStateMachine<T> where T : BaseState
    {
        public Stack<T> _states = new ();
        
        public override bool ChangeState(T state) {
            if (base.ChangeState(state)) {
                SaveState(state);
                return true;
            }

            return false;
        }

        public virtual void ReturnState() {
            if (_states.Count > 0)
            {
                var state= _states.Pop();
                
                if (ChangeStateInternal(_states.Peek()) == false)
                {
                    _states.Push(state);
                }
                
                _onStateChanged.Dispatch(_states.Peek());
            }
        }
        
        private void SaveState(T state) {
            _states.Push(state);
        }
    }
}