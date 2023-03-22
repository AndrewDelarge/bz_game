using System;
using game.core.Storage.Data.Character;
using UnityEngine;

namespace game.Gameplay.Characters.Common
{
    // TODO
    // Чото намудрил я тут с типами, над подмать мб можно упростить 
    public interface ICharacterAnimation<T, TEnum, TClipType> where T : IAnimationSet<TEnum, TClipType> where TEnum : Enum
    {
        TEnum currentAnimation { get; }
        IAnimationData<TEnum, TClipType> GetAnimationData(TEnum state);
        void PlayAnimation(TClipType animationClip, bool withExitTransition = false);
        void PlayAnimation(TEnum animationClip, bool withExitTransition = false);
        void StopAnimation();

        void SetMotionVelocityPercent(float percent);
        void Disable();
    }
}