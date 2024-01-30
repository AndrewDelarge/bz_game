using game.core.InputSystem;
using game.Gameplay.Common;

namespace game.Gameplay.Characters
{
    public abstract class CharacterState<T, TContext> : IBaseState
    {
        protected TContext context;

        public virtual void Init(TContext context)
        {
            this.context = context;
        }

        public virtual void OnChangedStateHandler<TObservered>(TObservered state) { }

        public virtual bool CheckEnterCondition() => true;

        public virtual bool CheckExitCondition() => true;

        public virtual void Enter() { }

        public virtual void Exit() { }

        public virtual void HandleState(float deltaTime) { }


        public virtual void HandleInput(InputData data) { }

        public virtual void Dispose() { }
    }
}