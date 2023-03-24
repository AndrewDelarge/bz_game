using game.core.storage.Data.Equipment;
using UnityEngine;

namespace game.Gameplay.Weapon {
	public abstract class EquipmentViewBase : MonoBehaviour {
		public abstract void Init(EquipmentData data);
	}
}