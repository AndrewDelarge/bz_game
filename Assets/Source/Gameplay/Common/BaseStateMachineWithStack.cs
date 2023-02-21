using System.Collections.Generic;

namespace game.Source.Gameplay.Common
{
    public class BaseStateMachineWithStack<T> : BaseStateMachine<T> where T : BaseState
    {
        public Stack<T> _states = new Stack<T>();

        public BaseStateMachineWithStack()
        {
            onStateChanged.Add(SaveState);
        }

        public void ReturnState()
        {
            if (_states.Count > 0)
            {
                var state= _states.Pop();
                
                if (ChangeStateInternal(_states.Peek()) == false)
                {
                    _states.Push(state);
                }
            }
        }
        
        private void SaveState(T state)
        {
            _states.Push(state);
        }
    }
}