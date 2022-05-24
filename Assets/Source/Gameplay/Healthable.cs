using System;
using UnityEngine;

namespace game.Source.Gameplay {
	public class Healthable : MonoBehaviour {
		[SerializeField] private float _maxHealth;
		[SerializeField] private float _currentHealth;
		[SerializeField] protected bool _initializeOnStart = false;

		protected HealthResource health;
		public Action die;
		
		protected virtual void Start() {
			if (! _initializeOnStart) 
				return;

			Init();
		}

		public virtual void Init() {
			Init(_currentHealth, _maxHealth);
		}
		
		public virtual void Init(float currentHealth, float maxHealth) {
			health = new HealthResource(maxHealth);
			health.Reduce(maxHealth - currentHealth);
		}
		
		public virtual void TakeDamage(HealthChange<DamageType> damage) {
			health.Reduce(damage.value);
			
			if (health.value == 0) {
				die?.Invoke();
			}
		}

		public virtual void Heal(HealthChange<HealType> heal) {
			health.Increase(heal.value);
		}

		public float GetHealth() {
			return health.value;
		}
	}
}