using UnityEngine;

namespace game.core.Storage.Data.Character
{
    public class CharacterAnimData : ScriptableObject
    {
        [SerializeField] public AnimationClip idle;
        [SerializeField] public AnimationClip walk;
        [SerializeField] public AnimationClip run;
    }
}