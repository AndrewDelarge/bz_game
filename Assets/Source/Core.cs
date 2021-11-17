using System;
using System.Collections.Generic;
using game.core.common;
using game.Source.Core.Common;

namespace game
{
    public class Core
    {   
        private static Core _instance = new Core();

        private Dictionary<Type, ICoreManager> _managers = new Dictionary<Type, ICoreManager>();
        private Queue<Action> _initQueue = new Queue<Action>();
        public static void Register<TConcrete>(ICoreManager manager) => _instance.RegisterInternal<TConcrete>(manager);

        public static TConcrete Get<TConcrete>() => _instance.InternalGet<TConcrete>();
        private void RegisterInternal<TConcrete>(ICoreManager manager)
        {
            _managers.Add(typeof(TConcrete), manager);
            
            if (manager is IInitalizeable initalizeable) {
                _initQueue.Enqueue(initalizeable.Init);
            }
        }

        private TConcrete InternalGet<TConcrete>()
        {
            _managers.TryGetValue(typeof(TConcrete), out ICoreManager manager);
            return (TConcrete) manager;
        }

        private void InternalStart()
        {
            while (_initQueue.Count > 0) {
                _initQueue.Dequeue().Invoke();
            }
        }
        
        public static void Start()
        {
            _instance.InternalStart();
        }
    }
}