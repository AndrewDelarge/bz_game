using game.core.Common;
using game.core.InputSystem;
using ILogger = game.core.Common.ILogger;

namespace game.Gameplay.Common
{
    public class BaseStateMachine<T> where T : BaseState {
        public T currentState => _currentState;
        
        public IWhistle<T> onStateChanged => _onStateChanged;
    
        protected T _currentState;
        private Whistle<T> _onStateChanged = new();
    
        public virtual void ChangeState(T state)
        {
            if (_currentState == state)
            {
                return;
            }

            if (ChangeStateInternal(state))
            {
                _onStateChanged.Dispatch(state);
            }
        }
    
        public void HandleState() {
            _currentState.HandleState();
        }
    
        public void HandleInput(InputData data) {
            _currentState.HandleInput(data);
        }

        protected virtual bool ChangeStateInternal(T state)
        {
            if (state.CheckExitCondition() == false)
            {
                AppCore.Get<ILogger>().Log($"Transition FROM \"{_currentState.GetType()}\" failed on check EXIT condition of this state");
                return false;
            }
            
            if (state.CheckEnterCondition() == false)
            {
                AppCore.Get<ILogger>().Log($"Transition TO \"{state.GetType()}\" failed on check ENTER condition of this state");
                return false;
            }
    
            _currentState?.Exit();
            _currentState = state;
            _currentState.Enter();

            return true;
        }
        
    }
}