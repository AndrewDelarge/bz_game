using System.Collections.Generic;
using game.core;
using game.core.storage.Data.Models;
using game.Gameplay;
using game.Gameplay.Characters.Common;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace game.Source.Gameplay.Weapon
{
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
                
                var view = Object.Instantiate(_model.viewTemplate, startPosition.transform.position, rotation);
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
    
    public abstract class Projectile
    {
        protected ICharacter _source;
        protected List<Healthable> _hitList;
        protected int _hitCount;
        protected bool _isStopped;
        protected ProjectileModel _model;
        protected List<ProjectileView> _views;

        
        public void Init(ProjectileModel model) {
            _model = model;
            _hitList = new List<Healthable>();
        }

        public int hitCount => _hitCount;
        public bool isStopped => _isStopped;
        public List<Healthable> hitList => _hitList;
        public ICharacter source => _source;

        public abstract HealthChange<DamageType> GetDamage();
        public abstract void Start(GameObject startPosition);

        public abstract void Stop();
        public abstract void Update(float deltaTime);
        
        public void ClearHitList()
        {
            _hitList.Clear();
            _hitCount = 0;
        }

        public void SetSource(ICharacter source)
        {
            _source = source;
        }

        public virtual void Dispose() {
            Stop();
            
            foreach (var view in _views) {
                view.Dispose();
                Object.DestroyImmediate(view.gameObject);
            }
            
            _views.Clear();
        } 
    }
}