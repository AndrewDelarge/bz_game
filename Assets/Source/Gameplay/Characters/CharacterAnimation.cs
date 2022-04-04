using game.core;
using UnityEngine;

namespace game.Source.Gameplay.Characters
{
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimation : BaseAnimator, ICharacterAnimation
    {
        private const string PARAM_VELOCITY = "velocity";
        
        [Header("Params")]
        [SerializeField] private float _velocityDampTime = .1f;

        [Header("States")] 
        [SerializeField] private string _actionName;

        private AnimatorOverrideController _overrideController;

        public void SetMotionVelocityPercent(float percent)
        {
            animator.SetFloat(PARAM_VELOCITY, percent, _velocityDampTime, Time.deltaTime);
        }
    }
}