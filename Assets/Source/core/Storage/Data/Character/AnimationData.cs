using System;
using NaughtyAttributes;
using UnityEngine;

namespace game.core.Storage.Data.Character {
	[Serializable]
	public class AnimationData<T> : IAnimationData<T, AnimationClip>  where T : Enum {
		[Dropdown("GetValues")]
		[SerializeField] private T _type;
		[SerializeField] private AnimationClip _clip;

		private DropdownList<T> _enumValues;
        
		public T type => _type;
		public AnimationClip clip => _clip;
		public float length => _clip.length;

		protected virtual DropdownList<T> GetValues() {
			if (_enumValues != null) {
				return _enumValues;
			}

			_enumValues = new DropdownList<T>();

			foreach (var value in Enum.GetValues(typeof(T))) {
				_enumValues.Add(value.ToString(), (T) value);
			}

			return _enumValues;
		}
	}
}