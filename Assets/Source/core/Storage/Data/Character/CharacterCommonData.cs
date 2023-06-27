using UnityEngine;

namespace game.core.Storage.Data.Character {
	public abstract class CharacterCommonData : ScriptableObject
	{
		[SerializeField] private CharacterAnimationSet _animationSet;
		[SerializeField] private float _normalSpeed = 1.3f;
		[SerializeField] private float _speedMultiplier = 2.8f;
		[SerializeField] private float _speedSmoothTime = 2f;
	    
		public CharacterAnimationSet animationSet => _animationSet;
		public float normalSpeed => _normalSpeed;
		public float speedMultiplier => _speedMultiplier;
		public float speedSmoothTime => _speedSmoothTime;
	}
}