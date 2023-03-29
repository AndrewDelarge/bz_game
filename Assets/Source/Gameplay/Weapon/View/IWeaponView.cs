using game.core.storage.Data.Equipment.Weapon;
using UnityEngine;

namespace game.Gameplay.Weapon {
	public interface IWeaponView {
		void Shot();

		GameObject GetMarkerPosition(string marker);
	}
}