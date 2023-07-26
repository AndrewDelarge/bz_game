using game.core;
using game.core.Common;
using game.core.Storage.Data.Character;
using game.core.view.animation;
using game.Gameplay.Characters.Common;
using UnityEngine;
using ILogger = game.core.Common.ILogger;

namespace game.Gameplay.Characters
{
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimation : BaseAnimator, ICharacterAnimation<CharacterAnimationSet, CharacterAnimationEnum, AnimationClip>
    {
        private readonly int PARAM_VELOCITY = Animator.StringToHash("velocity");
        
        private const int BASE_LAYER_ID = 3;
        private const int ACTION_LAYER_ID = 1;
        private const int ACTION_INMOVE_LAYER_ID = 2;
        
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
        private CharacterAnimationEnum _currentAnimation;
        private Whistle<CharacterAnimationEnum> _onAnimationComplete = new Whistle<CharacterAnimationEnum>();
        public CharacterAnimationEnum currentAnimation => _currentAnimation;
        public IWhistle<CharacterAnimationEnum> onAnimationComplete => _onAnimationComplete;

        public virtual void Init(CharacterAnimationSet characterAnimationSet)
        {
            base.Init();

            _defalutAnimationSet = characterAnimationSet;
            
            SetAnimationSet(characterAnimationSet);
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();

            animator.SetLayerWeight(ACTION_LAYER, animator.GetFloat(PARAM_VELOCITY) < .01f ? 1f : 0);
            animator.SetLayerWeight(ACTION_INMOVE_LAYER_ID, animator.GetFloat(PARAM_VELOCITY) > 0 ? 1 : 0);
        }

        public void PlayAnimation(CharacterAnimationEnum state, bool withExitTransition = false)
        {
            var data = GetAnimationData(state);

            if (data == null) {
                AppCore.Get<ILogger>().Log($"Animation <{state.ToString()}> not found in <{name}>");
                return;
            }

            _currentAnimation = state;
            base.PlayAnimation(data.clip, withExitTransition);
        }



        public override void StopAnimation() {
            _onAnimationComplete.Dispatch(_currentAnimation);
            _currentAnimation = CharacterAnimationEnum.NONE;
            
            base.StopAnimation();
        }

        public void SetMotionVelocityPercent(float percent, bool fast = false) {
            animator.SetFloat(PARAM_VELOCITY, percent, fast ? 0 : _velocityDampTime, Time.deltaTime);
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