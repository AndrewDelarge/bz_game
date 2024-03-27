using game.core.storage.Data.Models;
using UnityEngine;

namespace game.core.storage.Data.Equipment.Weapon {
	
	[CreateAssetMenu(fileName = "Shotgun weapon", menuName = "GameData/Equipment/ShotgunWeapon")]
	public class RifleWeaponData : WeaponData
	{
		public override IEquipment CreateModel()
		{
			var rifle = new RifleWeapon();
			rifle.Init(this);
			return rifle;
		}
	}
}