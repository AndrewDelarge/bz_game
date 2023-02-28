using UnityEngine;

namespace game.core.storage.Data.Equipment.Weapon {
	
	[CreateAssetMenu(fileName = "Handgun weapon", menuName = "GameData/Equipment/HandgunWeapon")]
	public class HandgunWeaponData : WeaponData {
		
		
	}
	
	
	public class WeaponData : EquipmentData
	{
		[SerializeField] private GameObject _view;

		public GameObject view => _view;
	}
}