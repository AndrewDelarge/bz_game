using UnityEngine;

namespace game.core.storage.Data.Equipment.Weapon {
	
	[CreateAssetMenu(fileName = "Shotgun weapon", menuName = "GameData/Equipment/ShotgunWeapon")]
	public class ShotgunWeaponData : WeaponData {
		[Header("Timings")] 
		[SerializeField] private float _shotTime = .65f;
		[SerializeField] private float _reloadTime = 3f;
		
	}
	
	
	public class WeaponData : EquipmentData
	{
		[SerializeField] private GameObject _view;

		public GameObject view => _view;
	}
}