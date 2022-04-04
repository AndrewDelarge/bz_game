using UnityEngine;

namespace game.core.Storage.Data.Character
{
    [CreateAssetMenu(fileName = "CharacterAnimData", menuName = "Character/AnimationData")]
    public class CharacterAnimData : ScriptableObject, ICharacterAnimationData
    {
        [SerializeField] private AnimationClip _idle;
        [SerializeField] private AnimationClip _walk;
        [SerializeField] private AnimationClip _run;

        [SerializeField] private AnimationClip _testClip;
        
        
        public AnimationClip idle => _idle;
        public AnimationClip walk => _walk;
        public AnimationClip run => _run;
        public AnimationClip testClip => _testClip;
    }

    public interface ICharacterAnimationData
    {
        public AnimationClip idle { get; }
        public AnimationClip walk { get; }
        public AnimationClip run { get; }
        public AnimationClip testClip { get; }
    }
}