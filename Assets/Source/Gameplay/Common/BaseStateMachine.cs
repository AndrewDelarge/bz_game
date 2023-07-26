using game.core.Common;
using game.core.InputSystem;
using ILogger = game.core.Common.ILogger;

namespace game.Gameplay.Common
{
    public class BaseStateMachine<T> where T : class, IBaseState {
        public T currentState => _currentState;
        public T prevState => _prevState;
        
        public IWhistle<T> onStateChanged => _onStateChanged;
    
        protected T _currentState;
        protected T _prevState;
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
    
        public void HandleState(float deltaTime) {
            _currentState.HandleState(deltaTime);
        }
    
        public void HandleInput(InputData data) {
            _currentState.HandleInput(data);
        }

        protected virtual bool ChangeStateInternal(T state)
        {
            if (_currentState != null && _currentState.CheckExitCondition() == false)
            {
                AppCore.Get<ILogger>().Log($"Transition FROM \"{_currentState.GetType()}\" failed on check EXIT condition of this state");
                return false;
            }
            
            if (state.CheckEnterCondition() == false)
            {
                AppCore.Get<ILogger>().Log($"Transition TO \"{state.GetType()}\" failed on check ENTER condition of this state");
                return false;
            }
            
            _prevState = _currentState;
            _prevState?.Exit();
            _currentState = state;
            _currentState.Enter();

            return true;
        }
        
    }
}