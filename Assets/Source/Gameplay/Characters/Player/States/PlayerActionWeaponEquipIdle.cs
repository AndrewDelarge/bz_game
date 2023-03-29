using game.core.InputSystem;
using game.core.Storage.Data.Character;
using game.Gameplay.Weapon;

namespace game.Gameplay.Characters.Player
{
    public class PlayerActionWeaponEquipIdle : PlayerStateBase<PlayerActionStateEnum, PlayerCharacterContext>
    {
        private WeaponStateMachine _weaponStateMachine;

        public override void Init(PlayerCharacterContext context)
        {
            base.Init(context);

            _weaponStateMachine = new WeaponStateMachine();
			
            var weaponContext = new WeaponStateContext(_weaponStateMachine, context.equipmentManger.currentEquipmentView, context.equipmentManger.currentEquipment);
			
            _weaponStateMachine.Init(weaponContext);
            _weaponStateMachine.ChangeState(WeaponStateEnum.IDLE);
            _weaponStateMachine.onStatesChanged.Add(WeaponChangeStateHandle);
        }
        
        public override void HandleState()
        {
            _weaponStateMachine.HandleState();
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
			
            if (currentWeaponState is WeaponStateEnum.AIM or WeaponStateEnum.RELOAD) {
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