using game.core.storage.Data.Models;
using UnityEngine;

namespace game.Gameplay.Weapon {
	public abstract class EquipmentViewBase : MonoBehaviour {
		public abstract void Init(IEquipment data);

		public virtual void Dispose() { }
	}
}