using game.core.common;
using game.core.level;
using UnityEngine;

namespace game.core
{
    public interface ILevelManager
    {
        void Add(IUpdatable updatable);
        T Get<T>();
        void SpawnDebugObject(Vector3 position, float size = 1f);

        public INavigator navigator { get; }
    }
}