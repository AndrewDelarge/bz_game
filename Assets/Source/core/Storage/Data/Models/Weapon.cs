using System;
using System.Collections.Generic;
using System.Linq;
using game.core.storage.Data.Equipment;
using game.core.storage.Data.Equipment.Weapon;
using game.Gameplay;
using game.Gameplay.Weapon;
using UnityEngine;

namespace game.core.storage.Data.Models
{
    public abstract class Weapon : IEquipment
    {
        private int _magazineCapacity;
        private int _currentMagazineAmount;

        private ProjectileModel _projectileModel;

        protected WeaponData _data;
        public float shotTime => _data.shotTime;
        public float reloadTime => _data.reloadTime;
        public int magazineCapacity => _data.magazineCapacity;
        public ProjectileModel projectileModel => _projectileModel;
        public Vector2 spreadX => _data.spreadX;
        public Vector2 spreadY => _data.spreadY;
        public int projectilesForShot => _data.projectilesForShot;
        public int currentMagazineAmount
        {
            set => _currentMagazineAmount = Math.Clamp(value, 0, _magazineCapacity);
            get => _currentMagazineAmount;
        }
        
        public abstract EquipmentData data { get; }
        public abstract IReadOnlyDictionary<WeaponStateEnum, WeaponStateBase> GetWeaponStates();
        public abstract Projectile GetProjectile();
        
        public virtual void Init(EquipmentData weaponData)
        {
            _data = (WeaponData) weaponData;
            _magazineCapacity = _data.magazineCapacity;
            
            _currentMagazineAmount = _magazineCapacity;
            
            _projectileModel = new ProjectileModel(_data.defaultProjectile);
        }
        

        public abstract HealthChange<DamageType> GetDamage();
        public GameObject GetFxByName(string name) => _data.fx.First(x => x.name == name).prefab;
    }
}