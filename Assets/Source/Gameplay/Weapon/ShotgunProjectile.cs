
using System.Collections.Generic;
using UnityEngine;

namespace game.Gameplay.Weapon {
	public class ShotgunProjectile : Projectile
	{
		private const float LIFE_TIME = 2f;

		private float _lifeTimeLeft = LIFE_TIME;

		private int PROJECTILE_COUNT = 10;

		public override void Start(GameObject startPosition)
		{
			_views = new List<ProjectileView>();
			var transformRotation = startPosition.transform.rotation;
			var rotation = new Quaternion(transformRotation.x, transformRotation.y,
				transformRotation.z, transformRotation.w);


			for (int i = 0; i < PROJECTILE_COUNT; i++)
			{
				rotation = startPosition.transform.rotation;
            
				rotation *= Quaternion.Euler(Random.Range(-20, 20), Random.Range(-20, 20), 0);
                
				var view = Object.Instantiate<ProjectileView>(_model.viewTemplate, startPosition.transform.position, rotation);
				view.onHitHealthable.Add(OnHitHandle);
				_views.Add(view);
			}
		}

		private void OnHitHandle(Healthable healthable, ProjectileView view) 
		{
			_hitCount++;
			_hitList.Add(healthable);
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
        
		public override HealthChange<DamageType> GetDamage()
		{
			return source.GetDamage();
		}
	}
}