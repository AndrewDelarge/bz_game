using System;
using System.Collections.Generic;
using game.core.InputSystem;
using game.Gameplay.Common;
using UnityEngine;
using ILogger = game.core.Common.ILogger;

namespace game.Gameplay.Weapon
{
    public class WeaponIdle : WeaponStateBase
    {
        public override void HandleInput(InputData data)
        {
            var aim = data.GetAction(InputActionType.AIM);
			
            if (aim is {value: {status: InputStatus.DOWN}}) {
                aim.isAbsorbed = true;

                _context.stateMachine.ChangeState(_context.states[typeof(WeaponAim)]);
            }
			
            var shot = data.GetAction(InputActionType.SHOT);

            if (shot is {value: {status: InputStatus.DOWN}})
            {
                shot.isAbsorbed = true;

                _context.stateMachine.ChangeState(_context.states[typeof(WeaponShot)]);
                return;
            }
			
			
            var reload = data.GetAction(InputActionType.SHOT);

            if (reload is {value: {status: InputStatus.DOWN}})
            {
                reload.isAbsorbed = true;

                _context.stateMachine.ChangeState(_context.states[typeof(WeaponReload)]);
            }
        }

    }

    public class WeaponAim : WeaponStateBase
    {
        public override void HandleInput(InputData data)
        {
            var aim = data.GetAction(InputActionType.AIM);
			
            if (aim is not {value: {status: InputStatus.PRESSED}}) {
                _context.stateMachine.ReturnState();
                return;
            }
            
            var shot = data.GetAction(InputActionType.SHOT);

            if (shot is {value: {status: InputStatus.DOWN}})
            {
                shot.isAbsorbed = true;

                _context.stateMachine.ChangeState(_context.states[typeof(WeaponShot)]);
                return;
            }
        }

        public override void HandleState()
        {
            AppCore.Get<ILogger>().Log("AIMING");
        }
    }
    
    public class WeaponShot : WeaponStateBase
    {
        
        public override void HandleState()
        {
            AppCore.Get<ILogger>().Log("BOOM!");
            
            _context.stateMachine.ReturnState();
        }
    }
    
    public class WeaponReload : WeaponStateBase
    {
        private float RELOAD_TIME = 5f;
        
        private float _startTime;

        public override bool CheckExitCondition()
        {
            return _startTime <= 0;
        }

        public override void Enter()
        {
            _startTime = RELOAD_TIME;
        }

        public override void HandleState()
        {
            _startTime -= Time.deltaTime;

            if (CheckExitCondition())
            {
                _context.stateMachine.ReturnState();
            }
            
            AppCore.Get<ILogger>().Log("Reload");
        }
    }
    
    
    public abstract class WeaponStateBase : BaseState
    {
        protected WeaponStateContext _context;
        
        public void Init(WeaponStateContext context)
        {
            _context = context;
        }
        
        public override void Enter()
        {
        }

        public override void Exit()
        {
        }

        public override void HandleState()
        {
        }

        public override void HandleInput(InputData data)
        {
        }
    }

    public class WeaponStateContext
    {
        public readonly BaseStateMachineWithStack<WeaponStateBase> stateMachine;
        public readonly Dictionary<Type, WeaponStateBase> states;


        public WeaponStateContext(BaseStateMachineWithStack<WeaponStateBase> stateMachine, Dictionary<Type, WeaponStateBase> states)
        {
            this.stateMachine = stateMachine;
            this.states = states;
        }
    }
}