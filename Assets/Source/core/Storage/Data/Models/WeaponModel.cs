using System;
using System.Collections.Generic;
using System.Linq;
using game.core.storage.Data.Equipment.Weapon;
using game.Gameplay.Weapon;
using UnityEngine;

namespace game.core.storage.Data.Models
{
    public abstract class WeaponModel : EquipmentModel
    {
        private float _shotTime;
        private float _reloadTime;
        private int _magazineCapacity;

        private int _currentMagazineAmount;

        private ProjectileModel _projectileModel;
        private Vector2 _spreadX;
        private Vector2 _spreadY;
        private int _projectilesForShot;

        protected WeaponData _data;

        public int currentMagazineAmount
        {
            set => _currentMagazineAmount = Math.Clamp(value, 0, _magazineCapacity);
            get => _currentMagazineAmount;
        }

        public float shotTime => _shotTime;
        public float reloadTime => _reloadTime;
        public int magazineCapacity => _magazineCapacity;
        public ProjectileModel projectileModel => _projectileModel;
        public Vector2 spreadX => _spreadX;
        public Vector2 spreadY => _spreadY;
        public int projectilesForShot => _projectilesForShot;


        public abstract IReadOnlyDictionary<WeaponStateEnum, WeaponStateBase> GetWeaponStates();

        protected WeaponModel(WeaponData weaponData) : base(weaponData) {
            _data = weaponData;
            _shotTime = weaponData.shotTime;
            _reloadTime = weaponData.reloadTime;
            _magazineCapacity = weaponData.magazineCapacity;
            _spreadX = weaponData.spreadX;
            _spreadY = weaponData.spreadY;
            _projectilesForShot = weaponData.projectilesForShot;
            

            _currentMagazineAmount = _magazineCapacity;
            
            _projectileModel = new ProjectileModel(weaponData.defaultProjectile);
        }

        public GameObject GetFxByName(string name) => _data.fx.First(x => x.name == name).prefab;

        
        public abstract Projectile GetProjectile();
    }
}