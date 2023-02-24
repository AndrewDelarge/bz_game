using game.core;
using game.core.Storage.Data.Character;
using game.Gameplay.Characters.Common;
using UnityEngine;

namespace game.Gameplay.Characters
{
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimation : BaseAnimator, ICharacterAnimation
    {
        private const string PARAM_VELOCITY = "velocity";
        
        private const string ANIMATION_IDLE_NAME = "empty_idle";
        private const string ANIMATION_WALK_NAME = "empty_walk";
        private const string ANIMATION_RUN_NAME = "empty_run";
        
        [Header("Params")]
        [SerializeField] private float _velocityDampTime = .1f;

        [Header("States")] 
        [SerializeField] private string _actionName;

        private AnimatorOverrideController _overrideController;

        public virtual void Init(CharacterAnimationSet characterAnimationSet)
        {
            base.Init();
            
            SetAnimationSet(characterAnimationSet);
        }
        
        public void SetMotionVelocityPercent(float percent)
        {
            animator.SetFloat(PARAM_VELOCITY, percent, _velocityDampTime, Time.deltaTime);
        }

        public void SetAnimationSet(CharacterAnimationSet characterAnimationSet)
        {
            overrideController[ANIMATION_IDLE_NAME] = characterAnimationSet.GetAnimationData(CharacterAnimationEnum.IDLE).clip;
            overrideController[ANIMATION_WALK_NAME] = characterAnimationSet.GetAnimationData(CharacterAnimationEnum.WALK).clip;
            overrideController[ANIMATION_RUN_NAME] = characterAnimationSet.GetAnimationData(CharacterAnimationEnum.RUN).clip;
        }

        public void Disable() {
            animator.enabled = false;
            enabled = false;
        }
    }
}