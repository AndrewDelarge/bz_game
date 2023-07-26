using System;
using game.core.Common;
using game.core.Storage.Data.Character;

namespace game.Gameplay.Characters.Common
{
    // TODO
    // Чото намудрил я тут с типами, над подмать мб можно упростить 
    public interface ICharacterAnimation<T, TEnum, TClipType> where T : IAnimationSet<TEnum, TClipType> where TEnum : Enum
    {
        TEnum currentAnimation { get; }
        IWhistle<TEnum> onAnimationComplete { get; }
        IAnimationData<TEnum, TClipType> GetAnimationData(TEnum state);
        void PlayAnimation(TClipType animationClip, bool withExitTransition = false);
        void PlayAnimation(TEnum animationClip, bool withExitTransition = false);
        void StopAnimation();

        void SetMotionVelocityPercent(float percent, bool fast = false);
        void Disable();
    }
}