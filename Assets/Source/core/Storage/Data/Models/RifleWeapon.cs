using System.Collections.Generic;
using game.core.storage.Data.Equipment;
using game.core.storage.Data.Equipment.Weapon;
using game.Gameplay;
using game.Gameplay.Weapon;

namespace game.core.storage.Data.Models
{
    public class RifleWeapon : Weapon
    {
        private Dictionary<WeaponStateEnum, WeaponStateBase> _weaponStates = new () {
            {WeaponStateEnum.IDLE, new WeaponIdle()},
            {WeaponStateEnum.AIM, new WeaponAim()},
            {WeaponStateEnum.RELOAD, new WeaponReload()},
            {WeaponStateEnum.SHOT, new WeaponShot()},
            {WeaponStateEnum.READY, new WeaponReady()},
        };
        public override EquipmentData data => _data;

        public override IReadOnlyDictionary<WeaponStateEnum, WeaponStateBase> GetWeaponStates() => _weaponStates;
        
        public override void Init(EquipmentData weaponData) {
            base.Init(weaponData);
            
            _data = (RifleWeaponData) weaponData;
        }
        
        public override Projectile GetProjectile()
        {
            var projectile = new ShotgunProjectile();
            projectile.Init(projectileModel);
            return projectile;
        }

        public override HealthChange<DamageType> GetDamage()
        {
            return new HealthChange<DamageType>(_data.damage, _data.damageType);
        }
    }
}