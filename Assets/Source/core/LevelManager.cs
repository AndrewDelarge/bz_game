using System;
using System.Collections.Generic;
using game.core.common;
using game.сore.Common;
using UnityEngine;

namespace game.core
{
    //TODO отвязатьот монобеха
    public class LevelManager : MonoBehaviour, ICoreManager
    {
        protected Dictionary<Type, IUpdatable> _updatables = new Dictionary<Type, IUpdatable>();
        
        public void Init()
        {
            AppCore.Register<LevelManager>(this);
        }

        public void Add(IUpdatable updatable) {
            _updatables.Add(updatable.GetType(), updatable);
        }

        private void Update() {
            foreach (var updatable in _updatables.Values) {
                updatable.Update(Time.deltaTime);
            }
        }

        public T Get<T>() {
            return (T) _updatables[typeof(T)];
        }

        public void SpawnDebugObject(Vector3 position, float size = 1f) {
            var debug = Instantiate(Resources.Load<GameObject>("_dev/debug/mark"), position, Quaternion.identity);
            debug.transform.localScale = new Vector3(size, size, size);
        }
    }

    public interface IUpdatable
    {
        public void Update(float deltaTime);
    }
}