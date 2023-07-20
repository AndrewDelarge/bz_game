using System.Collections.Generic;

namespace game.Gameplay.Common
{
    public class BaseStateMachineWithStack<TType, TState, TContext> : BaseStateMachineWithStack<TState> where TState : class, IBaseState<TType, TContext>
    {
        private Dictionary<TType, TState> _availableStates = new ();

        public virtual bool ChangeState(TType type) {
            if (_availableStates.ContainsKey(type)) {
                return ChangeState(_availableStates[type]);
            }

            return false;
        }
        
        public virtual void Init(List<TState> states, TContext context)
        {
            foreach (var state in states) {
                state.Init(context);
                
                _availableStates.Add(state.type, state);
            }
        }
    }
    
    public class BaseStateMachineWithStack<T> : BaseStateMachine<T> where T : class, IBaseState
    {
        private Stack<T> _states = new ();
        
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
                
                if (_states.Count == 0 || ChangeStateInternal(_states.Peek()) == false)
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