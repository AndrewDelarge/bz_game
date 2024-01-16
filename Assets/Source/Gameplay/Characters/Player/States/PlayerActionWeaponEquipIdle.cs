using game.core;
using game.core.InputSystem;
using game.core.level;
using game.core.storage;
using game.core.Storage.Data.Character;
using game.Gameplay.Characters.Common;
using game.Gameplay.Weapon;
using UnityEngine;

namespace game.Gameplay.Characters.Player
{
    public class PlayerActionWeaponEquipIdle : PlayerStateBase<PlayerActionStateEnum, PlayerCharacterContext>
    {
        private WeaponStateMachine _weaponStateMachine;

        public override void Init(PlayerCharacterContext context)
        {
            base.Init(context);

            _weaponStateMachine = new WeaponStateMachine();

            var projectileController = AppCore.Get<LevelManager>().levelController.Get<ProjectileController>();
            var weaponContext = new WeaponStateContext(_weaponStateMachine, context.equipmentManger.currentEquipmentView, context.equipmentManger.currentEquipment, context.character, projectileController);

            _weaponStateMachine.Init(weaponContext);
            _weaponStateMachine.ChangeState(WeaponStateEnum.IDLE);
            _weaponStateMachine.onStatesChanged.Add(WeaponChangeStateHandle);
        }
        
        public override void HandleState(float delta)
        {
            _weaponStateMachine.HandleState(delta);
        }

        private void WeaponChangeStateHandle(WeaponStateEnum last, WeaponStateEnum current) {
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
                case WeaponStateEnum.RELOAD:
                    context.animation.PlayAnimation(CharacterAnimationEnum.RELOAD, true);
                    break;
            }
        }

        public override void HandleInput(InputData data)
        {
            _weaponStateMachine.HandleInput(data);

            var currentWeaponState = _weaponStateMachine.currentStateType;

            if (currentWeaponState is WeaponStateEnum.AIM or WeaponStateEnum.SHOT) {
                var sprint = data.GetAction(InputActionType.SPRINT);
                if (sprint is {value: {status: InputStatus.DOWN}} || sprint is {value: {status: InputStatus.PRESSED}}) {
                    sprint.isAbsorbed = true;
                }
                
                var screenRes = AppCore.Get<CameraManager>().GetScreenResolutionDelta();
                var actualPointerPos = Input.mousePosition * screenRes;
                var ray = context.camera.ScreenPointToRay(actualPointerPos);

                if (Physics.Raycast(ray, out var hit, 1000, (int) GameLayers.AIM_PLANE)) {
                    var position = hit.point;
                    context.target.position = position;
                }
                
                var weapon = (WeaponView) context.equipmentManger.currentEquipmentView;
                var muzzle = weapon.GetMarkerPosition("muzzle");
                var targetDirection = context.target.position - muzzle.transform.position;
                
                var rotateAngle = Mathf.Atan2(targetDirection.x, targetDirection.z) * Mathf.Rad2Deg;
                context.movement.SetLockRotation(true);
                context.movement.Rotate(rotateAngle);
            }
            else {
                context.movement.SetLockRotation(false);
            }
            
            if (currentWeaponState is WeaponStateEnum.RELOAD) {
                var sprint = data.GetAction(InputActionType.SPRINT);
                if (sprint is {value: {status: InputStatus.PRESSED}}) {
                    sprint.isAbsorbed = true;
                }
                
                if (sprint is {value: {status: InputStatus.DOWN}}) {
                    _weaponStateMachine.ReturnState();
                    return;
                }
                
                if (context.mainStateMachine.currentState == CharacterStateEnum.RUN) {
                    context.mainStateMachine.ChangeState(CharacterStateEnum.WALK);
                }
            }
            else if (currentWeaponState != WeaponStateEnum.SHOT)
            {
                var kick = data.GetAction(InputActionType.KICK);
                if (kick is {value: {status: InputStatus.DOWN}}) {
                    kick.isAbsorbed = true;
                    
                    // _weaponStateMachine.ReturnState();
                    
                    context.actionStateMachine.ChangeState(PlayerActionStateEnum.KICK);
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
                _weaponStateMachine.ChangeState(WeaponStateEnum.IDLE);
            }
        }
    }
}