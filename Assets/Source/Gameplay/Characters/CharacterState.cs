using game.Gameplay.Common;

namespace game.Gameplay.Characters
{
    public abstract class CharacterState<T, TContext> : BaseState
    {
        protected TContext context;

        public virtual void Init(TContext context)
        {
            this.context = context;
        }

        public virtual void OnChangedStateHandler<TObservered>(TObservered state)
        {
            
        }
    }
}