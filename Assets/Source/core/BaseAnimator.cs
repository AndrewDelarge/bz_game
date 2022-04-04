using UnityEngine;

namespace game.core
{
    public class BaseAnimator : MonoBehaviour
    {
        protected const string PARAM_ACTION_TRIGGER = "action";
        protected const string DEFAULT_ACTION_STATE_NAME = "Action";
        protected const string DEFAULT_ACTION_ANIMATION_NAME = "empty";
        
        [SerializeField] protected Animator animator;

        protected AnimatorOverrideController overrideController;

        public virtual void Init()
        {
            overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
            animator.runtimeAnimatorController = overrideController;
        }

        public virtual void PlayAnimation(AnimationClip animationClip)
        {
            var anim = overrideController[DEFAULT_ACTION_ANIMATION_NAME];
            
            overrideController[DEFAULT_ACTION_ANIMATION_NAME] = animationClip;
            PlayState(DEFAULT_ACTION_STATE_NAME);
        }
        
        public virtual void PlayState(string name)
        {
            animator.Play(name);
        }
    }
}