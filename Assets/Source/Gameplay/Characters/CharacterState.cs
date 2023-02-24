using game.Gameplay.Common;

namespace game.Gameplay.Characters
{
    public abstract class CharacterState<T> : BaseState
    {
        protected CharacterContext context;

        public virtual void Init(CharacterContext context)
        {
            this.context = context;
        }

        public virtual void OnChangedStateHandler<TObservered>(TObservered state)
        {
            
        }
    }
}