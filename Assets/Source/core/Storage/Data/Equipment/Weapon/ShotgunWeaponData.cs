using game.core.storage.Data.Models;
using UnityEngine;

namespace game.core.storage.Data.Equipment.Weapon {
	
	[CreateAssetMenu(fileName = "Shotgun weapon", menuName = "GameData/Equipment/ShotgunWeapon")]
	public class ShotgunWeaponData : WeaponData
	{
		public override EquipmentModel CreateModel()
		{
			return new ShotgunWeaponModel(this);
		}
	}
}