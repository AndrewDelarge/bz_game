using game.core.InputSystem;
using game.Gameplay.Common;

namespace game.Gameplay.Weapon
{
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
}