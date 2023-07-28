using game.core.Storage.Data.Character;
using game.Source.core.Common;
using ILogger = game.core.Common.ILogger;

namespace game.Gameplay.Weapon
{
    public class WeaponReload : WeaponStateBase
    {
        private int _timerId;

        public override bool CheckEnterCondition() {
            return _context.data.magazineCapacity != _context.data.currentMagazineAmount;
        }

        public override void Enter() {
            _timerId = AppCore.Get<GameTimer>().SetTimeout(_context.data.reloadTime, OneTimeReload);
        }

        public override void Exit() {
            AppCore.Get<GameTimer>().KillTimeout(_timerId);
        }

        private void OneTimeReload() {
            _context.data.currentMagazineAmount = _context.data.magazineCapacity;
            _context.stateMachine.ReturnState();
            
            AppCore.Get<ILogger>().Log("Reloaded!");
        }
    }
}