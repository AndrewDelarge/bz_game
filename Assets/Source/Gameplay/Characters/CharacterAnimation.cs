using UnityEngine;

namespace game.Source.Gameplay.Characters
{
    public class CharacterAnimation : MonoBehaviour
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