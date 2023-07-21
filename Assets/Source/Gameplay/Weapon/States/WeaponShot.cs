using game.core;
using UnityEngine;
using ILogger = game.core.Common.ILogger;

namespace game.Gameplay.Weapon
{
    public class WeaponShot : WeaponStateBase {
        private const float FROM_IDLE_SHOT_DELAY = .07f;
        private IWeaponView _view;
        
        private float _endTime;
        private float _shotDelayTime;
        private bool _isDone;
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

            _isDone = false;
            _endTime = _context.data.shotTime;
            _shotDelayTime = _context.stateMachine.currentStateType == WeaponStateEnum.AIM ? 0 : FROM_IDLE_SHOT_DELAY;
        }

        public override void HandleState(float deltaTime) {
            _shotDelayTime -= Time.deltaTime;
            
            if (_shotDelayTime <= 0 && _isDone == false) {
                Shot();
                return;
            }
            
            _endTime -= Time.deltaTime;
            
            if (_endTime <= 0)
            {
                _context.stateMachine.ReturnState();
            }
        }

        private void Shot() {
            _context.data.currentMagazineAmount -= 1;

            var levelManager = AppCore.Get<LevelManager>();
            var projectileManager = levelManager.Get<ProjectileManager>();
            var projectile = _context.data.GetProjectile();
            
            projectile.SetSource(_context.owner);
            projectileManager.Launch(projectile, _context.data.projectilesForShot, _view.GetMarkerPosition("muzzle"), _context.data.spreadX, _context.data.spreadY);

            _view.Shot();

            AppCore.Get<ILogger>().Log($"AMMO ({_context.data.currentMagazineAmount}/{_context.data.magazineCapacity})");
            
            _isDone = true;
        }
    }
}