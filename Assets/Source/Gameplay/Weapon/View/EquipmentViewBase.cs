using game.core.storage.Data.Models;
using UnityEngine;

namespace game.Gameplay.Weapon {
	public abstract class EquipmentViewBase : MonoBehaviour {
		public abstract void Init(EquipmentModel data);

		public virtual void Dispose() { }
	}
}