using UnityEngine;
using ILogger = game.core.Common.ILogger;

namespace game.Gameplay.Weapon
{
    public class WeaponReload : WeaponStateBase
    {
        private float _startTime;

        public override bool CheckExitCondition()
        {
            return _startTime <= 0;
        }

        public override bool CheckEnterCondition()
        {
            return _context.data.magazineCapacity != _context.data.currentMagazineAmount;
        }

        public override void Enter()
        {
            _startTime = _context.data.reloadTime;
        }

        public override void HandleState()
        {
            _startTime -= Time.deltaTime;

            if (CheckExitCondition())
            {
                _context.data.currentMagazineAmount = _context.data.magazineCapacity;
                _context.stateMachine.ReturnState();
                return;
            }
            
            AppCore.Get<ILogger>().Log("Reload...");
        }
    }
}