using game.Gameplay.Characters.Common;
using UnityEngine;

namespace game.Gameplay.Characters
{
    public abstract class CharacterState<T> : BaseState
    {
        protected CharacterContext context;

        public virtual void Init(CharacterContext context)
        {
            this.context = context;
        }

        public virtual void OnChangedStateHandler(T state)
        {
            // Break current state? 
        }
    }
}