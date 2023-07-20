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

        public virtual void OnChangedStateHandler<TObservered>(TObservered state)
        {
            
        }

        public virtual void Enter()
        {
            throw new System.NotImplementedException();
        }

        public virtual void Exit()
        {
            throw new System.NotImplementedException();
        }

        public virtual void HandleState(float deltaTime)
        {
            throw new System.NotImplementedException();
        }


        public virtual void HandleInput(InputData data)
        {
            throw new System.NotImplementedException();
        }
    }
}