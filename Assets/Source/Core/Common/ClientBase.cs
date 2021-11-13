using System;
using game.core.InputSystem;
using UnityEngine;

namespace game.core.common
{
    public abstract class ClientBase : MonoBehaviour
    {
        private void Awake()
        {
            // 
        }

        private void Start()
        {
            init();
        }

        private void init()
        {
            CoreInit();
        }

        protected abstract void CoreInit();
    }
}