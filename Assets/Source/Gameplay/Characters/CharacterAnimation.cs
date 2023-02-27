using game.core;
using game.core.Storage.Data.Character;
using game.Gameplay.Characters.Common;
using UnityEngine;
using ILogger = game.core.Common.ILogger;

namespace game.Gameplay.Characters
{
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimation : BaseAnimator, ICharacterAnimation<CharacterAnimationSet, CharacterAnimationEnum, AnimationClip>
    {
        private const string PARAM_VELOCITY = "velocity";
        
        private const string ANIMATION_IDLE_NAME = "empty_idle";
        private const string ANIMATION_WALK_NAME = "empty_walk";
        private const string ANIMATION_RUN_NAME = "empty_run";
        
        [Header("Params")]
        [SerializeField] private float _velocityDampTime = .1f;

        [Header("States")] 
        [SerializeField] private string _actionName;

        protected CharacterAnimationSet _currentAnimationSet;
        protected CharacterAnimationSet _defalutAnimationSet;
        private AnimatorOverrideController _overrideController;


        public virtual void Init(CharacterAnimationSet characterAnimationSet)
        {
            base.Init();

            _defalutAnimationSet = characterAnimationSet;
            
            SetAnimationSet(characterAnimationSet);
        }

        public void PlayAnimation(CharacterAnimationEnum state)
        {
            var data = GetAnimationData(state);

            if (data != null)
            {
                base.PlayAnimation(data.clip);
            }
            
            AppCore.Get<ILogger>().Log($"Animation <{state.ToString()}> not found in <{name}>");
        }
        
        public void SetMotionVelocityPercent(float percent)
        {
            animator.SetFloat(PARAM_VELOCITY, percent, _velocityDampTime, Time.deltaTime);
        }

        public void SetAnimationSet(CharacterAnimationSet characterAnimationSet)
        {
            _currentAnimationSet = characterAnimationSet;
            
            ApplyAnimationSet();
        }
        
        public IAnimationData<CharacterAnimationEnum, AnimationClip> GetAnimationData(CharacterAnimationEnum state)
        {
            if (_currentAnimationSet == _defalutAnimationSet)
            {
                return _currentAnimationSet.GetAnimationData(state);
            }

            var data = _currentAnimationSet.GetAnimationData(state);

            if (data == null)
            {
                data = _defalutAnimationSet.GetAnimationData(state);
            }

            return data;
        }

        public void Disable() {
            animator.enabled = false;
            enabled = false;
        }



        private void ApplyAnimationSet()
        {
            overrideController[ANIMATION_IDLE_NAME] = _currentAnimationSet.GetAnimationData(CharacterAnimationEnum.IDLE).clip;
            overrideController[ANIMATION_WALK_NAME] = _currentAnimationSet.GetAnimationData(CharacterAnimationEnum.WALK).clip;
            overrideController[ANIMATION_RUN_NAME] = _currentAnimationSet.GetAnimationData(CharacterAnimationEnum.RUN).clip;
        }
    }
}