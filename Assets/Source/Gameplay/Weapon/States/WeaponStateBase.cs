using game.core.InputSystem;
using game.Gameplay.Common;

namespace game.Gameplay.Weapon
{
    public abstract class WeaponStateBase : IBaseState
    {
        protected WeaponStateContext _context;
        
        public virtual void Init(WeaponStateContext context)
        {
            _context = context;
        }

        public virtual bool CheckEnterCondition() => true;

        public virtual bool CheckExitCondition() => true;

        public virtual void Enter() { }

        public virtual void Exit() { }

        public virtual void HandleState(float deltaTime) { }

        public virtual void HandleInput(InputData data) { }
    }

    public enum WeaponStateEnum {
        NONE = -1,
        IDLE = 0,
        AIM = 1,
        SHOT = 2,
        RELOAD = 3
    }
}