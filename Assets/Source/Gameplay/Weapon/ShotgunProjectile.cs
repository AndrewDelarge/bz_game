
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace game.Gameplay.Weapon {
	public class ShotgunProjectile : Projectile
	{
		private const float LIFE_TIME = 1f;

		private float _lifeTimeLeft = LIFE_TIME;

		private Dictionary<Healthable, List<ProjectileView>> _hitDictionary = new Dictionary<Healthable, List<ProjectileView>>();

		public override void Start(Vector3 startPosition, Quaternion rotation)
		{
			var view = Object.Instantiate<ProjectileView>(_model.viewTemplate, startPosition, rotation);
			view.onHitHealthable.Add(OnHitHandle);
			_views.Add(view);
		}

		private void OnHitHandle(Healthable healthable, ProjectileView view) 
		{
			_hitCount++;
			_hitList.Add(healthable);

			if (_hitDictionary.ContainsKey(healthable)) {
				_hitDictionary[healthable].Add(view);
			} else {
				_hitDictionary.Add(healthable, new List<ProjectileView>() {view});
			}
			
			healthable.TakeDamage(GetDamage());

			view.Stop();
			view.onHitHealthable.Remove(OnHitHandle);
		}

		public override void Stop() {
			foreach (var view in _views) {
				view.Stop();
			}
            
			_isStopped = true;
		}

		public override void Update(float deltaTime)
		{
			foreach (var pair in _hitDictionary) {
				if (pair.Key.GetHealth() <= 0) {
					AddForce(pair.Key, pair.Value[0]);
					
					_hitDictionary.Remove(pair.Key);
				}
			}
			if (_lifeTimeLeft < 0 || _isStopped) {
				Stop();
				return;
			}

			_lifeTimeLeft -= deltaTime;

			foreach (var view in _views)
			{
				if (view.isStopped) {
					continue;
				}
                
				view.Move(Vector3.forward * _model.speed * Time.deltaTime);
			}
		}
        
		public override HealthChange<DamageType> GetDamage() {
			var damage = source.GetDamage();
			damage.ChangeValue(damage.value / _count);
			return damage;
		}
		
		private void AddForce(Healthable healthable, ProjectileView view) {
			var impulsePower = Math.Clamp(10 * _hitDictionary[healthable].Count, 10, 100);
			var rigidbody = healthable.GetComponentInChildren<Rigidbody>();
			rigidbody.AddForce(view.transform.forward * (impulsePower), ForceMode.Impulse);
		}
	}
}