using System;
using UnityEngine;

namespace game.core.view.animation
{
    public class BaseAnimator : MonoBehaviour
    {
        protected readonly int PARAM_ACTION_BOOL = Animator.StringToHash("actionBool");
        protected readonly int PARAM_TRANSITION_BOOL = Animator.StringToHash("transition");
        protected readonly int PARAM_EXIT_TRANSITION_BOOL = Animator.StringToHash("exitTransition");
        public const int ACTION_LAYER = 1;

        protected const string DEFAULT_ACTION_STATE_NAME = "Action";
        protected const string DEFAULT_ACTION_ANIMATION_NAME = "empty";
        
        [SerializeField] protected Animator animator;

        protected AnimatorOverrideController overrideController;

        protected float _animationTime = 0;
        
        public virtual void Init()
        {
            overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
            animator.runtimeAnimatorController = overrideController;
        }

        protected virtual void LateUpdate()
        {
            if (_animationTime > 0) {
                _animationTime -= Time.deltaTime;
                if (_animationTime <= 0)
                {
                    StopAnimation(true);
                }
            }
        }

        public virtual void PlayAnimation(AnimationClip animationClip, bool enterTransition = false)
        {
            if (_animationTime > 0) {
                StopAnimation(false);
            }
            
            overrideController[DEFAULT_ACTION_ANIMATION_NAME] = animationClip;
            if (enterTransition) {
                animator.SetBool(PARAM_TRANSITION_BOOL, true);
            }
            
            animator.SetBool(PARAM_ACTION_BOOL, true);
            animator.Update(0);
            _animationTime = animationClip.isLooping ? float.PositiveInfinity : animationClip.length;
        }

        public virtual void StopAnimation(bool exitTransition)
        {
            animator.SetBool(PARAM_ACTION_BOOL, false);
            animator.SetBool(PARAM_TRANSITION_BOOL, false);
            animator.SetBool(PARAM_EXIT_TRANSITION_BOOL, exitTransition);
            if (animator.isActiveAndEnabled) {
                animator.Update(0);
            }

            _animationTime = 0;
        }
        
        public virtual void PlayState(string name)
        {
            animator.Play(name);
        }
    }
}