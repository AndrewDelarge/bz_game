using System.Collections.Generic;
using UnityEngine;

namespace game.core.Storage.Data.Character
{
    [CreateAssetMenu(fileName = "CharacterAnimSet", menuName = "GameData/Character/AnimationSet")]
    public class CharacterAnimationSet : ScriptableObject, IAnimationSet<CharacterAnimationEnum>
    {
        [SerializeField] private List<AnimationData<CharacterAnimationEnum>> _animationSet;

        public AnimationData<CharacterAnimationEnum> GetAnimationData(CharacterAnimationEnum id) {
            return _animationSet.Find(x => x.type == id);
        }
    }


    public enum CharacterAnimationEnum {
        IDLE,
        WALK,
        RUN,
        KICK,
    }
}