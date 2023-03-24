using System;
using System.Collections.Generic;
using game.Gameplay.Weapon;
using UnityEngine;
using Object = UnityEngine.Object;

namespace game.core.storage.Data.Equipment.Weapon {
	
	[CreateAssetMenu(fileName = "Shotgun weapon", menuName = "GameData/Equipment/ShotgunWeapon")]
	public class ShotgunWeaponData : WeaponData {
		[Header("Timings")] 
		[SerializeField] private float _shotTime = .65f;
		[SerializeField] private float _reloadTime = 3f;
		
	}
	
	
	public class WeaponData : EquipmentData
	{
		[SerializeField] private EquipmentViewBase _view;
		[SerializeField] private List<WeaponFxData> _fx; 
		public EquipmentViewBase view => _view;

		public GameObject GetFxByName(string name) => _fx.Find(x => x.name == name)?.prefab;

		[Serializable]
		class WeaponFxData {
			[SerializeField] private string _name;
			[SerializeField] private GameObject _prefab;

			public string name => _name;
			public GameObject prefab => _prefab;
		}
	}
}