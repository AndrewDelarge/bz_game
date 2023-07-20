﻿using UnityEngine;
using ILogger = game.core.Common.ILogger;

namespace game.Gameplay.Weapon
{
    public class WeaponReload : WeaponStateBase
    {
        private float _startTime;

        public bool CheckExitCondition()
        {
            return _startTime <= 0;
        }

        public bool CheckEnterCondition()
        {
            return _context.data.magazineCapacity != _context.data.currentMagazineAmount;
        }

        public override void Enter()
        {
            _startTime = _context.data.reloadTime;
        }

        public override void HandleState(float delta)
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