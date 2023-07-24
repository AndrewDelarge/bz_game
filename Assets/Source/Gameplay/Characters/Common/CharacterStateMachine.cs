using System;
using System.Collections.Generic;
using game.core.Common;
using game.Gameplay.Common;

namespace game.Gameplay.Characters.Common
{
    public class CharacterStateMachine<T, TContext> : BaseStateMachine<CharacterState<T, TContext>>  where T : Enum 
    {
        private Dictionary<T, CharacterState<T, TContext>> _states;
        private Dictionary<T, CharacterState<T, TContext>> _statesOverrides = new ();
        private T _currentState;
        private TContext _context;
        
        private Whistle<T> _onStateChange;
        public IReadOnlyDictionary<T, CharacterState<T, TContext>> states => _states;
        public new T currentState => _currentState;
        public new IWhistle<T> onStateChanged => _onStateChange;

        public CharacterStateMachine(TContext context, Dictionary<T, CharacterState<T, TContext>> states) {
            _context = context;
            _states = states;
            _onStateChange = new Whistle<T>();

            foreach (var characterState in _states.Values) {
                characterState.Init(_context);
            }
        }
        
        public void ChangeState(T state) {
            var characterState = GetState(state);
            
            if (characterState == null) {
                AppCore.Get<ILogger>().Log($"State <{state.ToString()}> not register in CharacterStateMachine");
                return;
            }
            
            base.ChangeState(characterState);
            _currentState = state;
        }

        public void ReplaceState(T type, CharacterState<T, TContext> state) {
            if (_statesOverrides.ContainsKey(type)) {
                _statesOverrides.Remove(type);
            }
            
            state.Init(_context);
            
            _statesOverrides.Add(type, state);

            if (currentState.Equals(type))
            {
                ChangeState(type);
            }
        }
        
        public void OnStateChangeHandler<TObservered>(TObservered state) {
            base.currentState.OnChangedStateHandler(state);
        }

        private CharacterState<T, TContext> GetState(T type) {
            return _statesOverrides.ContainsKey(type)
                ? _statesOverrides[type]
                : _states.ContainsKey(type)
                    ? _states[type]
                    : null;
        }
    }
}