using System.Collections.Generic;
using UnityEngine;

namespace game.core.Storage.Data.Character
{
    [CreateAssetMenu(fileName = "CharacterAnimSet", menuName = "GameData/Character/AnimationSet")]
    public class CharacterAnimationSet : ScriptableObject, IAnimationSet<CharacterAnimationEnum, AnimationClip>
    {
        [SerializeField] private List<AnimationData<CharacterAnimationEnum>> _animationSet;

        public IAnimationData<CharacterAnimationEnum, AnimationClip> GetAnimationData(CharacterAnimationEnum id) {
            return _animationSet.Find(x => x.type == id);
        }
    }


    public enum CharacterAnimationEnum {
        NONE = -1,
        IDLE = 0,
        WALK = 1,
        RUN = 2,
        KICK = 3,
        AIM = 4,
        SHOT = 5,
        RELOAD = 6,
        ABILITY = 7,
        ABILITY1 = 8,
        ABILITY2 = 9,
        DEATH = 30
    }
}