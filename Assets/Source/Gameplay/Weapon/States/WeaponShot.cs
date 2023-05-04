using game.core;
using UnityEngine;
using ILogger = game.core.Common.ILogger;

namespace game.Gameplay.Weapon
{
    public class WeaponShot : WeaponStateBase {
        private IWeaponView _view;
        
        private float _endTime;
        public override void Init(WeaponStateContext context) {
            base.Init(context);
            
            _view = context.view;
        }

        public override bool CheckEnterCondition()
        {
            if (_context.data.currentMagazineAmount > 0)
            {
                return true;
            }

            _context.stateMachine.ChangeState(WeaponStateEnum.RELOAD);
            
            return false;
        }

        public override void Enter()
        {
            base.Enter();
            
            _endTime = _context.data.shotTime;
            
            _view.Shot();
            
            _context.data.currentMagazineAmount -= 1;

            var levelManager = AppCore.Get<LevelManager>();
            var projectileManager = levelManager.Get<ProjectileManager>();
            var projectile = _context.data.GetProjectile();
            projectile.SetSource(_context.owner);
            projectileManager.Launch(projectile, _view.GetMarkerPosition("muzzle"));

            AppCore.Get<ILogger>().Log($"AMMO ({_context.data.currentMagazineAmount}/{_context.data.magazineCapacity})");
        }

        public override void HandleState()
        {
            _endTime -= Time.deltaTime;

            if (_endTime <= 0)
            {
                AppCore.Get<ILogger>().Log("BOOM!");
                
                _context.stateMachine.ReturnState();
            }
        }
    }
}