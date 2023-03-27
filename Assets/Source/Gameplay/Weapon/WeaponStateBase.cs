using game.core.InputSystem;
using game.core.storage.Data.Models;
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

                _context.stateMachine.ChangeState(WeaponStateEnum.AIM);
            }
			
            var shot = data.GetAction(InputActionType.SHOT);

            if (shot is {value: {status: InputStatus.DOWN}})
            {
                shot.isAbsorbed = true;

                _context.stateMachine.ChangeState(WeaponStateEnum.SHOT);
                return;
            }
			
			
            var reload = data.GetAction(InputActionType.RELOAD);

            if (reload is {value: {status: InputStatus.DOWN}})
            {
                reload.isAbsorbed = true;

                _context.stateMachine.ChangeState(WeaponStateEnum.RELOAD);
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

                _context.stateMachine.ChangeState(WeaponStateEnum.SHOT);
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
        private IWeaponView _view;
        
        private float _endTime;
        public override void Init(WeaponStateContext context) {
            base.Init(context);
            
            _view = context.view;
        }

        public override void Enter()
        {
            base.Enter();
            
            _endTime = _context.data.shotTime;
            
            _view.Shot();
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
    
    public class WeaponReload : WeaponStateBase
    {
        private float _startTime;

        public override bool CheckExitCondition()
        {
            return _startTime <= 0;
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
                _context.stateMachine.ReturnState();
                return;
            }
            
            AppCore.Get<ILogger>().Log("Reload");
        }
    }
    
    
    public abstract class WeaponStateBase : BaseState
    {
        protected WeaponStateContext _context;
        
        public virtual void Init(WeaponStateContext context)
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

    public enum WeaponStateEnum {
        NONE = -1,
        IDLE = 0,
        AIM = 1,
        SHOT = 2,
        RELOAD = 3
    }

    public class WeaponStateContext
    {
        public readonly WeaponStateMachine stateMachine;
        public readonly IWeaponView view;
        public readonly WeaponModel data;
        
        public WeaponStateContext(WeaponStateMachine stateMachine, EquipmentViewBase view, EquipmentModel data)
        {
            this.stateMachine = stateMachine;
            this.view = (IWeaponView) view;
            this.data = (WeaponModel) data;
        }
    }
}