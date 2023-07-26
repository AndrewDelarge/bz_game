using System;
using System.Collections.Generic;
using game.core.common;
using game.сore.Common;

namespace game
{
    public class AppCore
    {   
        private static AppCore _instance = new();
        
        private Dictionary<Type, ICoreManager> _managers = new Dictionary<Type, ICoreManager>();
        private Queue<Action> _initQueue = new Queue<Action>();

        private bool isStarted;
        
        public static void Register<TConcrete>(ICoreManager manager) => _instance.RegisterInternal<TConcrete>(manager);

        public static TConcrete Get<TConcrete>() => _instance.InternalGet<TConcrete>();

        private void DisposeInternal() {
            _managers.Clear();
        }
        
        private void RegisterInternal<TConcrete>(ICoreManager manager)
        {
            _managers.Add(typeof(TConcrete), manager);
            
            if (manager is IInitalizeable initalizeable) {
                if (isStarted == false) {
                    _initQueue.Enqueue(initalizeable.Init);
                } else {
                    initalizeable.Init();
                }
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

            isStarted = true;
        }
        
        public static void Start()
        {
            _instance.InternalStart();
        }
        
        public static void Dispose()
        {
            _instance.DisposeInternal();
        }
    }
}