using UnityEngine;

namespace game.core.common
{
    public abstract class ClientBase : MonoBehaviour
    {
        private void Start()
        {
            init();
        }

        private void init()
        {
            CoreInit();
            CoreStart();
            GameStart();
        }

        protected abstract void CoreInit();
        protected abstract void CoreStart();
        protected abstract void GameStart();
    }
}