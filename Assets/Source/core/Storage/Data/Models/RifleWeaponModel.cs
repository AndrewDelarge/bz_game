using System.Collections.Generic;
using System.Linq;
using game.core.storage.Data.Equipment.Weapon;
using game.Gameplay;
using game.Gameplay.Weapon;
using UnityEngine;

namespace game.core.storage.Data.Models
{
    public class RifleWeaponModel : WeaponModel
    {
        private Dictionary<WeaponStateEnum, WeaponStateBase> _weaponStates = new () {
            {WeaponStateEnum.IDLE, new WeaponIdle()},
            {WeaponStateEnum.AIM, new WeaponAim()},
            {WeaponStateEnum.RELOAD, new WeaponReload()},
            {WeaponStateEnum.SHOT, new WeaponShot()},
            {WeaponStateEnum.READY, new WeaponReady()},
        };
        public RifleWeaponModel(WeaponData weaponData) : base(weaponData) {
            _data = (RifleWeaponData) weaponData;
        }

        public override IReadOnlyDictionary<WeaponStateEnum, WeaponStateBase> GetWeaponStates() => _weaponStates;
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