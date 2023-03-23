using System.Collections.Generic;
using game.core.storage.Data.Equipment.Weapon;
using UnityEngine;

namespace game.Gameplay.Weapon {
	public class ShotgunWeapon : WeaponBase
	{
		private const string SHOT_FX = "shot";
		private const string AMMO_DROP_FX = "ammo_drop";
		private const string MUZZLE_MARKER = "muzzle";
		
		private int _amountAmmo;
		private int _loadedAmmo;
		private ShotgunWeaponData _data;
		private Markers _markers;

		private Dictionary<string, GameObject> _fx;

		private List<GameObject> _particles = new ();
		
		public void Init(ShotgunWeaponData data) {
			_data = data;

			// _fx = _data.fx;
		}
		
		public void Shot() {
			if (_loadedAmmo <= 0) {
				return;
			}
			
			if (_fx.ContainsKey(SHOT_FX) == false) {
				return;
			}

			var marker = _markers.GetMarker(MUZZLE_MARKER);
					
			if (marker != null) {
				_particles.Add(Object.Instantiate(_fx[SHOT_FX], marker.markerObject.transform));
			}
			_particles.Add(Object.Instantiate(_fx[AMMO_DROP_FX]));
		}
	}

	public class WeaponBase {

	}
}