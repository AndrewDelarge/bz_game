using UnityEngine;

namespace game.Source.Gameplay.Characters
{
    public interface ICharacterAnimation {
        void PlayAnimation(AnimationClip animationClip);

        void SetMotionVelocityPercent(float percent);

        void Disable();
    }
}