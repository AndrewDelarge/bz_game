using game.core.Common;
using UnityEngine;
using ILogger = game.core.Common.ILogger;

namespace game.Gameplay.Characters.Common
{
    public class BaseStateMachine<T> where T : BaseState {
    public T currentState;

    public IWhistle<T> onStateChanged => _onStateChanged;

    private Whistle<T> _onStateChanged = new();

    public virtual void ChangeState(T state)
    {
        if (currentState == state)
        {
            return;
        }

        if (state.CheckEnterCondition() == false)
        {
            AppCore.Get<ILogger>().Log($"Transition to \"{state.GetType()}\" failed on check condition of this state");
            return;
        }

        currentState?.Exit();
        currentState = state;
        currentState.Enter();

        _onStateChanged.Dispatch(state);
    }

    }
}