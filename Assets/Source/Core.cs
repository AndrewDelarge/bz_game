using System;
using System.Collections.Generic;
using game.core.common;

namespace game
{
    public class Core
    {   
        private static Core _instance = new Core();

        private Dictionary<Type, ICoreManager> _managers = new Dictionary<Type, ICoreManager>();

        public static void Register<TConcrete>(ICoreManager manager) => _instance.RegisterInternal<TConcrete>(manager);

        public static TConcrete Get<TConcrete>() => _instance.InternalGet<TConcrete>();
        private void RegisterInternal<TConcrete>(ICoreManager manager)
        {
            _managers.Add(typeof(TConcrete), manager);
        }

        private TConcrete InternalGet<TConcrete>()
        {
            _managers.TryGetValue(typeof(TConcrete), out ICoreManager manager);
            return (TConcrete) manager;
        }
    }
}