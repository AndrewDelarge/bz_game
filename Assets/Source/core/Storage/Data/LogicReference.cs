using UnityEngine;

namespace game.core.storage.Data.Abilities {
	public abstract class LogicReference<T> : ScriptableObject {
		public abstract T value { get; }
	}
}