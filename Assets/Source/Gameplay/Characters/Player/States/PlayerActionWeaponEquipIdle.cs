using System.Collections.Generic;
using game.core.InputSystem;
using game.core.Storage.Data.Character;
using game.core.storage.Data.Equipment.Weapon;
using game.Gameplay.Weapon;
using UnityEngine;
using Object = UnityEngine.Object;

namespace game.Gameplay.Characters.Player
{
    public class PlayerActionWeaponEquipIdle : PlayerStateBase<PlayerActionStateEnum, PlayerCharacterContext>
    {
        private WeaponStateMachine _stateMachine;

        private GameObject _tmpWeaponView;
        
        private Dictionary<WeaponStateEnum, WeaponStateBase> _weaponStates = new ()
        {
            {WeaponStateEnum.IDLE, new WeaponIdle()},
            {WeaponStateEnum.AIM, new WeaponAim()},
            {WeaponStateEnum.RELOAD, new WeaponReload()},
            {WeaponStateEnum.SHOT, new WeaponShot()},
        };

        public override void Init(PlayerCharacterContext context)
        {
            base.Init(context);

            _stateMachine = new WeaponStateMachine();
			
            var weaponContext = new WeaponStateContext(_stateMachine);
			
            _stateMachine.Init(weaponContext, _weaponStates);
            _stateMachine.ChangeState(WeaponStateEnum.IDLE);
            _stateMachine.onStatesChanged.Add(WeaponStateHandle);
        }

        public override void Enter() {
            base.Enter();

            var weaponData = (WeaponData) context.equipmentManger.currentEquipment;

            var bone = context.boneListenerManager.bones[BoneListenerManager.CharacterBone.RIGHT_HAND_WEAPON];
            
            _tmpWeaponView = Object.Instantiate(weaponData.view, bone.transform);
        }

        public override void Exit()
        {
            base.Exit();
            
            Object.Destroy(_tmpWeaponView);
        }

        public override void HandleState()
        {
            _stateMachine.HandleState();
            
            var currentWeaponState = _stateMachine.currentState.GetType();
        }

        private void WeaponStateHandle(WeaponStateEnum last, WeaponStateEnum current) {
            switch (current) {
                case WeaponStateEnum.IDLE:
                    context.animation.StopAnimation();
                    break;
                case WeaponStateEnum.SHOT:
                    var transition = last == WeaponStateEnum.IDLE;
                    context.animation.PlayAnimation(CharacterAnimationEnum.SHOT, transition);
                    break;
                case WeaponStateEnum.AIM:
                    context.animation.PlayAnimation(CharacterAnimationEnum.AIM, true);
                    break;
            }
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
                _stateMachine.ChangeState(WeaponStateEnum.IDLE);
            }
        }
    }
}