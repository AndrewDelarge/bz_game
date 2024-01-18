using UnityEngine;
using ILogger = game.core.Common.ILogger;

namespace game.Gameplay.Weapon
{
    public class WeaponShot : WeaponStateBase {
        private const float FROM_IDLE_SHOT_DELAY = .1f;
        private IWeaponView _view;
        
        private float _endTime;
        private float _shotDelayTime;
        private bool _isDone;
        private float _spreadMultiplier;
        private WeaponStateEnum _lastState;
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
            _isDone = false;
            _endTime = _context.data.shotTime;
            _lastState = _context.stateMachine.currentStateType;
            _spreadMultiplier = _lastState is WeaponStateEnum.IDLE or WeaponStateEnum.READY ? 5 : 1;
            _shotDelayTime = _context.stateMachine is {currentStateType: WeaponStateEnum.AIM or WeaponStateEnum.READY} ? 0 : FROM_IDLE_SHOT_DELAY;
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
                if (_lastState == WeaponStateEnum.IDLE) {
                    _context.stateMachine.ChangeState(WeaponStateEnum.READY);
                    return;
                }
            }
        }

        private void Shot() {
            _context.data.currentMagazineAmount -= 1;

            var projectileController = _context.projectileController;
            var projectile = _context.data.GetProjectile();
            
            projectile.SetSource(_context.owner);
            projectileController.Launch(projectile, _context.data.projectilesForShot, _view.GetMarkerPosition("muzzle"), _context.data.spreadX * _spreadMultiplier, _context.data.spreadY * _spreadMultiplier);

            _view.Shot();

            AppCore.Get<ILogger>().Log($"AMMO ({_context.data.currentMagazineAmount}/{_context.data.magazineCapacity})");
            
            _isDone = true;
        }
    }
}