using UnityEngine;

namespace game.Source.Gameplay.Characters
{
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimation : MonoBehaviour, ICharacterAnimation
    {
        private const string PARAM_VELOCITY = "velocity";
        
        [SerializeField] private Animator _animator;
        
        [Header("Params")]
        [SerializeField] private float _velocityDampTime = .1f;
        
        public void SetMotionVelocityPercent(float percent)
        {
            _animator.SetFloat(PARAM_VELOCITY, percent, _velocityDampTime, Time.deltaTime);
        }
    }
}