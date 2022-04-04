using game.core;
using game.core.Storage.Data.Character;
using UnityEngine;

namespace game.Source.Gameplay.Characters
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

        public virtual void Init(CharacterAnimData animationSet)
        {
            base.Init();
            
            SetAnimationSet(animationSet);
        }
        
        public void SetMotionVelocityPercent(float percent)
        {
            animator.SetFloat(PARAM_VELOCITY, percent, _velocityDampTime, Time.deltaTime);
        }

        private void SetAnimationSet(CharacterAnimData animationSet)
        {
            overrideController[ANIMATION_IDLE_NAME] = animationSet.idle;
            overrideController[ANIMATION_WALK_NAME] = animationSet.walk;
            overrideController[ANIMATION_RUN_NAME] = animationSet.run;
        } 
    }
}