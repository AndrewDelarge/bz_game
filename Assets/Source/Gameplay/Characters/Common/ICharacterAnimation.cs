using System;
using game.core.Storage.Data.Character;
using UnityEngine;

namespace game.Gameplay.Characters.Common
{
    // TODO
    // Чото намудрил я тут с типами, над подмать мб можно упростить 
    public interface ICharacterAnimation<T, TEnum, TClipType> where T : IAnimationSet<TEnum, TClipType> where TEnum : Enum
    {
        void PlayAnimation(TClipType animationClip);
        void PlayAnimation(TEnum animationClip);

        void SetMotionVelocityPercent(float percent);

        IAnimationData<TEnum, TClipType> GetAnimationData(TEnum state);

        void Disable();
    }
}