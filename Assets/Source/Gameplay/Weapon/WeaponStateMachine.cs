using System.Collections.Generic;
using game.core.Common;
using game.Gameplay.Common;

namespace game.Gameplay.Weapon {
	public class WeaponStateMachine : BaseStateMachineWithStack<WeaponStateBase> {
		private IReadOnlyDictionary<WeaponStateEnum, WeaponStateBase> _availableStates;
		private Whistle<WeaponStateEnum, WeaponStateEnum> _onStatesChanged = new ();
		private WeaponStateEnum _currentStateType;

		// первый аргумент - прошлый стейт, второй - текущий
		public IWhistle<WeaponStateEnum, WeaponStateEnum> onStatesChanged => _onStatesChanged;
		
		public WeaponStateEnum currentStateType => _currentStateType;
		public void Init(WeaponStateContext context) {
			_availableStates = context.data.GetWeaponStates();

			foreach (var state in _availableStates.Values) {
				state.Init(context);
			}
		}


		public override void ReturnState() {
			base.ReturnState();
			
			foreach (var state in _availableStates) {
				if (state.Value == currentState) {
					_onStatesChanged.Dispatch(_currentStateType, state.Key);
					_currentStateType = state.Key;
					break;
				}
			}
		}

		public void ChangeState(WeaponStateEnum state) {
			if (_availableStates.ContainsKey(state) == false) {
				return;
			}

			
			if (base.ChangeState(_availableStates[state])) {
				var lastState = _currentStateType;
				_currentStateType = state;
				_onStatesChanged.Dispatch(lastState, _currentStateType);
			}
		}
	}
}