using game.Source.core.Common;
using UnityEngine;
using ILogger = game.Source.core.Common.ILogger;

namespace game.Source.Gameplay.Characters.Common
{
    public class BaseStateMachine<T> where T : BaseState {
    public T currentState;

    public ISignal<T> onStateChanged => _onStateChanged;

    private Signal<T> _onStateChanged = new();

    public virtual void ChangeState(T state)
    {
        if (currentState == state)
        {
            return;
        }

        if (state.CheckEnterCondition() == false)
        {
            GCore.Get<ILogger>().Log($"Transition to \"{state.GetType()}\" failed on check condition of this state");
            return;
        }

        currentState?.Exit();
        currentState = state;
        currentState.Enter();

        _onStateChanged.Dispatch(state);
    }

    }
}