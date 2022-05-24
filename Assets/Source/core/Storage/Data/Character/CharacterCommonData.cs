using UnityEngine;

namespace game.core.Storage.Data.Character {
	[CreateAssetMenu(menuName = "Create CommonData", fileName = "Character/CommonData", order = 0)]
	public class CharacterCommonData : ScriptableObject {
		[SerializeField] private float _normalSpeed = 1.3f;
		[SerializeField] private float _speedMultiplier = 2.8f;
		[SerializeField] private float _speedSmoothTime = 2f;
	    
		public float normalSpeed => _normalSpeed;
		public float speedMultiplier => _speedMultiplier;
		public float speedSmoothTime => _speedSmoothTime;
	}
}