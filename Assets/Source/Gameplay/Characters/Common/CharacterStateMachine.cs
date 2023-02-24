using System;
using System.Collections.Generic;
using game.core.Common;
using game.Gameplay.Common;

namespace game.Gameplay.Characters.Common
{
    public class CharacterStateMachine<T> : BaseStateMachine<CharacterState<T>>  where T : Enum
    {
        private Dictionary<T, CharacterState<T>> _states;
        private Dictionary<T, CharacterState<T>> _statesOverrides = new Dictionary<T, CharacterState<T>>();
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
            var characterState = GetState(state);
            
            if (characterState == null) {
                AppCore.Get<ILogger>().Log($"State <{state.ToString()}> not register in CharacterStateMachine");
                return;
            }
            
            base.ChangeState(_states[state]);
            _currentState = state;
        }

        public void ReplaceState(T type, CharacterState<T> state) {
            if (_statesOverrides.ContainsKey(type)) {
                _statesOverrides.Remove(type);
            }
            
            _statesOverrides.Add(type, state);
            
            ChangeState(type);
        }
        
        public void OnStateChangeHandler<TObservered>(TObservered state) {
            base.currentState.OnChangedStateHandler(state);
        }

        private CharacterState<T> GetState(T type) {
            return _statesOverrides.ContainsKey(type)
                ? _statesOverrides[type]
                : _states.ContainsKey(type)
                    ? _states[type]
                    : null;
        }
    }
}