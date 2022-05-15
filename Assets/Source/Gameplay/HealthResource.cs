using UnityEngine;

namespace game.Source.Gameplay {
	public class HealthResource : Resource<float> {
		protected float _maxHealth;
		public HealthResource(float maxHealth) {
			_value = maxHealth;
			_maxHealth = maxHealth;
		}
		
		public override bool Increase(float value) {
			if (value <= 0) {
				return false;
			}
			
			_value = Mathf.Clamp(_value + value, 0, _maxHealth);
			return true;
		}

		public override bool Reduce(float value) {
			if (value <= 0) {
				return false;
			}
			
			_value = Mathf.Clamp(_value - value, 0, _value);
			return true;
		}
	}
}