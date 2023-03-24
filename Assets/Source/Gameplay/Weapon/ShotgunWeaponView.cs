using System.Collections.Generic;
using game.core.storage.Data.Equipment;
using game.core.storage.Data.Equipment.Weapon;
using UnityEngine;

namespace game.Gameplay.Weapon {
	public class ShotgunWeaponView : EquipmentViewBase, IWeaponView
	{
		private const string SHOT_FX = "shot";
		private const string AMMO_DROP_FX = "ammo_drop";
		private const string MUZZLE_MARKER = "muzzle";
		
		private ShotgunWeaponData _data;
		[SerializeField] private Markers _markers;


		private List<GameObject> _particles = new ();
		
		public override void Init(EquipmentData data) {
			_data = (ShotgunWeaponData) data;
		}
		
		public void Shot() {
			var fx = _data.GetFxByName(SHOT_FX);
			if (fx == null) {
				return;
			}

			var marker = _markers.GetMarker(MUZZLE_MARKER);

			if (marker != null) {
				_particles.Add(Instantiate(_data.GetFxByName(SHOT_FX), marker.markerObject.transform));
			}
			// _particles.Add(Object.Instantiate(_fx[AMMO_DROP_FX]));
		}
	}
}