using game.core.Common;
using game.core.InputSystem;
using ILogger = game.core.Common.ILogger;

namespace game.Gameplay.Common
{
    public class BaseStateMachine<T> where T : BaseState {
        public T currentState => _currentState;
        
        public IWhistle<T> onStateChanged => _onStateChanged;
    
        protected T _currentState;
        protected Whistle<T> _onStateChanged = new();
    
        public virtual bool ChangeState(T state)
        {
            if (_currentState == state)
            {
                return false;
            }

            var isStateChanged = ChangeStateInternal(state);
            if (isStateChanged)
            {
                _onStateChanged.Dispatch(state);
            }
            
            return isStateChanged;
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