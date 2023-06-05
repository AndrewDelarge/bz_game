using System.Collections.Generic;
using game.core.storage.Data.Models;
using game.Gameplay.Characters.Common;
using UnityEngine;
using Object = UnityEngine.Object;

namespace game.Gameplay.Weapon
{
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