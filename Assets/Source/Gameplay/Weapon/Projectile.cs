using System.Collections.Generic;
using game.core;
using game.core.common;
using game.core.storage.Data.Models;
using game.Gameplay;
using game.Gameplay.Characters.Common;
using game.сore.Common;
using UnityEngine;

namespace game.Source.Gameplay.Weapon
{
    public class ProjectileManager : IUpdatable
    {
        protected List<Projectile> _projectiles = new List<Projectile>();
        
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

                if (projectile.hitCount == 0) {
                    continue;
                }

                var hit = projectile.hitList[projectile.hitCount];
                
                hit.TakeDamage(projectile.GetDamage());
            }
        }
    }

    public class ShotgunProjectile : Projectile
    {
        public override HealthChange<DamageType> GetDamage()
        {
            return source.GetDamage();
        }

        public override void Start(GameObject startPosition)
        {
            _view = Object.Instantiate(_model.viewTemplate, startPosition.transform.position, startPosition.transform.rotation);
            
        }

        public override void Stop()
        {
        }

        public override void Update(float deltaTime)
        {
            _view.transform.Translate( Vector3.forward * _model.speed * Time.deltaTime );
        }
    }
    
    public abstract class Projectile
    {
        protected ICharacter _source;
        protected List<Healthable> _hitList;
        protected int _hitCount;
        protected bool _isStopped;
        protected ProjectileModel _model;
        protected ProjectileView _projectileView;
        protected GameObject _startPosition;
        protected ProjectileView _view;
        
        public void Init(ProjectileModel model) {
            _model = model;
        }

        public int hitCount => _hitCount;
        public bool isStopped => _isStopped;
        public List<Healthable> hitList => _hitList;
        public ICharacter source => _source;

        public abstract HealthChange<DamageType> GetDamage();
        public abstract void Start(GameObject startPosition);

        public abstract void Stop();
        public abstract void Update(float deltaTime);
    }
}