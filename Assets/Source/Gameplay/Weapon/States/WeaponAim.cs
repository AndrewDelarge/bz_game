using game.core.Common;
using game.core.InputSystem;

namespace game.Gameplay.Weapon
{
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
            
            var reload = data.GetAction(InputActionType.RELOAD);

            if (reload is {value: {status: InputStatus.DOWN}})
            {
                reload.isAbsorbed = true;

                _context.stateMachine.ChangeState(WeaponStateEnum.RELOAD);
            }
        }

        public override void HandleState()
        {
            AppCore.Get<ILogger>().Log("AIMING");
        }
    }
}