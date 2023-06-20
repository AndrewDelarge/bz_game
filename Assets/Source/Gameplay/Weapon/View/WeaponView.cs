using System.Collections.Generic;
using game.core.storage.Data.Models;
using UnityEngine;

namespace game.Gameplay.Weapon {
	public class WeaponView : EquipmentViewBase, IWeaponView
	{
		private const string SHOT_FX = "shot";
		private const string AMMO_DROP_FX = "ammo_drop";
		private const string MUZZLE_MARKER = "muzzle";
		
		private WeaponModel _data;
		[SerializeField] private Markers _markers;


		private List<GameObject> _particles = new ();
		
		public override void Init(EquipmentModel data) {
			_data = (WeaponModel) data;
			
			_markers.Init();
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

		public GameObject GetMarkerPosition(string markerName) {
			var marker = _markers.GetMarker(markerName);
			if (marker != null) {
				return marker.markerObject.gameObject;
			}

			return null;
		}
	}
}