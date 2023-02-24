using System;
using game.Gameplay.Characters;
using game.Gameplay.Common;
using NaughtyAttributes;
using UnityEngine;

namespace game.core.storage.Data.Character {
	public abstract class State<T, T1> : ScriptableObject where T : CharacterState<T1> where  T1 : Enum {
		[Dropdown("GetValues")]
		[SerializeField] private T1 _overrideStateType;
		public abstract T GetState();


		private DropdownList<T1> _enumValues;
        
		public T1 overrideStateType => _overrideStateType;

		protected virtual DropdownList<T1> GetValues() {
			if (_enumValues != null) {
				return _enumValues;
			}

			_enumValues = new DropdownList<T1>();

			foreach (var value in Enum.GetValues(typeof(T1))) {
				_enumValues.Add(value.ToString(), (T1) value);
			}

			return _enumValues;
		}
	}
}