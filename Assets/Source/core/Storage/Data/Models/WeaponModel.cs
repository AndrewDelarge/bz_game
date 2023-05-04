using System;
using System.Collections.Generic;
using game.core.storage.Data.Equipment.Weapon;
using game.Gameplay.Weapon;

namespace game.core.storage.Data.Models
{
    public abstract class WeaponModel : EquipmentModel
    {
        private float _shotTime;
        private float _reloadTime;
        private int _magazineCapacity;

        private int _currentMagazineAmount;

        private ProjectileModel _projectileModel;
        
        public int currentMagazineAmount
        {
            set => _currentMagazineAmount = Math.Clamp(value, 0, _magazineCapacity);
            get => _currentMagazineAmount;
        }

        public float shotTime => _shotTime;
        public float reloadTime => _reloadTime;
        public int magazineCapacity => _magazineCapacity;
        public ProjectileModel projectileModel => _projectileModel;

        public abstract IReadOnlyDictionary<WeaponStateEnum, WeaponStateBase> GetWeaponStates();

        protected WeaponModel(WeaponData weaponData) : base(weaponData) {
            _shotTime = weaponData.shotTime;
            _reloadTime = weaponData.reloadTime;
            _magazineCapacity = weaponData.magazineCapacity;

            _currentMagazineAmount = _magazineCapacity;
            _projectileModel = new ProjectileModel(weaponData.defaultProjectile);
        }

        public abstract Projectile GetProjectile();
    }
}