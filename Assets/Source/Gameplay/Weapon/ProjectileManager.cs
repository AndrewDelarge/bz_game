using System.Collections.Generic;
using game.core;
using UnityEngine;

namespace game.Gameplay.Weapon {
	public class ProjectileManager : IUpdatable
	{
		protected List<Projectile> _projectiles = new ();
		protected List<Projectile> _projectilesToDestroy = new ();
        
		public void Launch(Projectile projectile, GameObject startPosition)
		{
			projectile.Start(startPosition);
			_projectiles.Add(projectile);
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
					healthable.TakeDamage(projectile.GetDamage());
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