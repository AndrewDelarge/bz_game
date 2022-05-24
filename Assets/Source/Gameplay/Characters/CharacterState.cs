using game.Source.Gameplay.Characters.Common;
using UnityEngine;

namespace game.Source.Gameplay.Characters
{
    public abstract class CharacterState : BaseState
    {
        protected CharacterContext context;

        public virtual void Init(CharacterContext context) {
            this.context = context;
        }
    }
}