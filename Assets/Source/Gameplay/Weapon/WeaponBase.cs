using System.Collections.Generic;
using game.core.storage.Data.Equipment.Weapon;
using UnityEngine;

namespace game.Gameplay.Weapon {
	public class ShotgunWeapon : WeaponBase {

		private int _amountAmmo;
		private int _loadedAmmo;
		private ShotgunWeaponData _data;

		private Dictionary<string, GameObject> _fx;

		public void Init(ShotgunWeaponData data) {
			_data = data;

			_fx = _data.fx;
		}
		
		public void Shot() {
			if (_loadedAmmo > 0) {
				
			}
		}
	}
	
	public class WeaponBase {

	}
}