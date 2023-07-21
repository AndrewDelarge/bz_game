using game.core.InputSystem;

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
}