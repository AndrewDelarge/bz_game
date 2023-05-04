using System.Collections.Generic;
using System.Linq;
using game.core.storage.Data.Equipment.Weapon;
using game.Gameplay;
using game.Gameplay.Weapon;
using UnityEngine;

namespace game.core.storage.Data.Models
{
    public class ShotgunWeaponModel : WeaponModel
    {
        protected ShotgunWeaponData _data;
        
        private Dictionary<WeaponStateEnum, WeaponStateBase> _weaponStates = new () {
            {WeaponStateEnum.IDLE, new WeaponIdle()},
            {WeaponStateEnum.AIM, new WeaponAim()},
            {WeaponStateEnum.RELOAD, new WeaponReload()},
            {WeaponStateEnum.SHOT, new WeaponShot()},
        };
        public ShotgunWeaponModel(WeaponData weaponData) : base(weaponData) {
            _data = (ShotgunWeaponData) weaponData;
        }

        public GameObject GetFxByName(string name) => _data.fx.First(x => x.name == name).prefab;

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