using System;
using System.Collections.Generic;

namespace game.Source.Gameplay.Characters.Common
{
    public class CharacterStateMachine<T> : BaseStateMachine<CharacterState<T>>  where T : Enum
    {
        private Dictionary<T, CharacterState<T>> _states;
        public IReadOnlyDictionary<T, CharacterState<T>> states => _states;
        
        public CharacterStateMachine(Dictionary<T, CharacterState<T>> states) {
            _states = states;
        }

        public void ChangeState(T state) {
            if (_states.ContainsKey(state)) {
                base.ChangeState(_states[state]);
            }
        }
        
        public void OnStateChangeHandler(T state) {
            currentState.OnChangedStateHandler(state);
        }
    }
}