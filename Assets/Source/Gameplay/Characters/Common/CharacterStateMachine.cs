using System;
using System.Collections.Generic;
using game.core.Common;
using game.Source.Gameplay.Common;

namespace game.Gameplay.Characters.Player.Common
{
    public class CharacterStateMachine<T> : BaseStateMachine<CharacterState<T>>  where T : Enum
    {
        private Dictionary<T, CharacterState<T>> _states;
        private T _currentState;
        private Whistle<T> _whistle;
        public IReadOnlyDictionary<T, CharacterState<T>> states => _states;
        public new T currentState => _currentState;
        public new IWhistle<T> onStateChanged => _whistle;

        public CharacterStateMachine(Dictionary<T, CharacterState<T>> states) {
            _states = states;
            _whistle = new Whistle<T>();
        }

        public void ChangeState(T state) {
            if (_states.ContainsKey(state)) {
                base.ChangeState(_states[state]);
            }
        }
        
        public void OnStateChangeHandler<TObservered>(TObservered state) {
            base.currentState.OnChangedStateHandler(state);
        }
    }
}