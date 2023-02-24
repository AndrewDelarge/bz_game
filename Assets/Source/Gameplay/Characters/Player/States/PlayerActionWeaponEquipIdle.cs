using System;
using System.Collections.Generic;
using game.core.InputSystem;
using game.Gameplay.Weapon;
using game.Gameplay.Common;

namespace game.Gameplay.Characters.Player
{
    public class PlayerActionWeaponEquipIdle : PlayerStateBase<PlayerActionStateEnum>
    {
        private BaseStateMachineWithStack<WeaponStateBase> _stateMachine;
        private Dictionary<Type, WeaponStateBase> _weaponStates = new ()
        {
            {typeof(WeaponIdle), new WeaponIdle()},
            {typeof(WeaponAim), new WeaponAim()},
            {typeof(WeaponReload), new WeaponReload()},
            {typeof(WeaponShot), new WeaponShot()},
        };

        public override void Init(CharacterContext context)
        {
            base.Init(context);

            _stateMachine = new BaseStateMachineWithStack<WeaponStateBase>();
			
            var weaponContext = new WeaponStateContext(_stateMachine, _weaponStates);
			
            foreach (var state in _weaponStates.Values)
            {
                state.Init(weaponContext);
            }
            
            _stateMachine.ChangeState(_weaponStates[typeof(WeaponIdle)]);
        }

        public override void Enter() {
            base.Enter();
            
        }

        public override void HandleState()
        {
            _stateMachine.HandleState();
        }

        public override void HandleInput(InputData data)
        {
            _stateMachine.HandleInput(data);

            var currentWeaponState = _stateMachine.currentState.GetType();
			
            if (currentWeaponState == typeof(WeaponAim) || currentWeaponState == typeof(WeaponReload)) {
                var sprint = data.GetAction(InputActionType.SPRINT);
                if (sprint is {value: {status: InputStatus.PRESSED}}) {
                    sprint.isAbsorbed = true;
                }
                
                if (sprint is {value: {status: InputStatus.DOWN}}) {
                    _stateMachine.ReturnState();
                    return;
                }
                
                if (context.mainStateMachine.currentState == CharacterStateEnum.RUN) {
                    context.mainStateMachine.ChangeState(CharacterStateEnum.WALK);
                }
            }
        }

        public override void OnChangedStateHandler<T>(T state) 
        {
            if (state is not CharacterStateEnum stateEnum)
            {
                return;
            }

			
            if (stateEnum == CharacterStateEnum.RUN)
            {
                _stateMachine.ChangeState(_weaponStates[typeof(WeaponIdle)]);
            }
        }
    }
}