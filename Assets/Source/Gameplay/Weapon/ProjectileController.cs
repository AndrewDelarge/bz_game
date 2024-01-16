using System.Collections.Generic;
using game.core;
using UnityEngine;

namespace game.Gameplay.Weapon {
	public class ProjectileController : IUpdatable
	{
		protected List<Projectile> _projectiles = new ();
		protected List<Projectile> _projectilesToDestroy = new ();
        
		public void Launch(Projectile projectile, int count, GameObject startPosition, Vector2 spreadX, Vector2 spreadY)
		{
			for (int i = 0; i < count; i++)
			{			
				var rotation = startPosition.transform.rotation * Quaternion.Euler(Random.Range(spreadX.x, spreadX.y), Random.Range(spreadY.x, spreadY.y), 0);
			
				projectile.Start(startPosition.transform.position, rotation);
				_projectiles.Add(projectile);
			}
		}

		public void Update(float deltaTime)
		{
			foreach (var projectile in _projectiles)
			{
				if (projectile.isStopped == false) {
					projectile.Update(deltaTime);
				}
				else {
					_projectilesToDestroy.Add(projectile);
				}

				if (projectile.hitList.Count == 0) {
					continue;
				}

				foreach (var healthable in projectile.hitList)
				{
					// healthable.TakeDamage(projectile.GetDamage());
				}

				projectile.ClearHitList();
			}

			foreach (var projectile in _projectilesToDestroy) {
				projectile.Dispose();
				_projectiles.Remove(projectile);
			}
            
			_projectilesToDestroy.Clear();
		}
	}
}