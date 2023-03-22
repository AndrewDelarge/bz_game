using System;
using UnityEngine;

namespace game.core
{
    public class BaseAnimator : MonoBehaviour
    {
        protected readonly int PARAM_ACTION_TRIGGER = Animator.StringToHash("action");
        protected readonly int PARAM_ACTION_WITH_TRANSITION_TRIGGER = Animator.StringToHash("actionWithTransition");
        protected readonly int PARAM_STOP_ACTION_TRIGGER = Animator.StringToHash("endAction");
        protected const int ACTION_LAYER = 1;

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

        protected virtual void Update()
        {
            if (_animationTime > 0)
            {
                _animationTime -= Time.deltaTime;

                if (_animationTime <= 0)
                {
                    StopAnimation();
                }
            }
        }

        public virtual void PlayAnimation(AnimationClip animationClip, bool withExitTransition = false)
        {
            overrideController[DEFAULT_ACTION_ANIMATION_NAME] = animationClip;
            animator.SetTrigger(withExitTransition ? PARAM_ACTION_WITH_TRANSITION_TRIGGER : PARAM_ACTION_TRIGGER);
            _animationTime = animationClip.isLooping ? float.PositiveInfinity : animationClip.length;
        }

        public virtual void StopAnimation()
        {
            animator.SetTrigger(PARAM_STOP_ACTION_TRIGGER);
            
            if (animator.IsInTransition(ACTION_LAYER)) {
                return;
            }

            _animationTime = 0;
        }
        
        public virtual void PlayState(string name)
        {
            animator.Play(name);
        }
    }
}