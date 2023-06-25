using System;
using game.core.Common;
using UnityEngine;

namespace game.Gameplay {
	public class Healthable : MonoBehaviour {
		[SerializeField] private float _maxHealth;
		[SerializeField] private float _currentHealth;
		[SerializeField] protected bool _initializeOnStart = false;
		[SerializeField] private ParticleSystem fx;
		protected HealthResource health;
		public Whistle die = new Whistle();
		
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
			if (fx != null) { 
				fx.Play();
			}
			if (health.value == 0) {
				die.Dispatch();
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