using System;
using System.Collections.Generic;
using game.core.common;
using game.core.InputSystem;
using game.core.storage.Data.Level;
using game.gameplay.control;
using UnityEngine;
using Object = UnityEngine.Object;

namespace game.core.level
{
    public class LevelController
    {
        private Dictionary<Type, IUpdatable> _updatables = new Dictionary<Type, IUpdatable>();
        private Navigator _navigator;

        public INavigator navigator => _navigator;
        
        public void Init(LevelData data)
        {
            var inputListener = Object.Instantiate(Resources.Load<InputListener>("InputListener"));
            var playerCharacter = Object.Instantiate(data.playerCharacter, Vector3.zero, Quaternion.identity);
            var camera = Object.Instantiate(data.camera, Vector3.zero, Quaternion.identity);

            playerCharacter.SetCamera(camera.camera);
            camera.SetTarget(playerCharacter.currentTransform);
            AppCore.Get<CameraManager>().AddCamera(camera);
            AppCore.Get<LevelManager>().characterController.RegisterCharacter(playerCharacter);
            AppCore.Get<IInputManager>().RegisterControlable(playerCharacter);

            _navigator = new Navigator();
            _navigator.Init();
        }

        public void Add(IUpdatable updatable) {
            _updatables.Add(updatable.GetType(), updatable);
        }

        public void Update(float deltaTime) {
            foreach (var updatable in _updatables.Values) {
                updatable.Update(deltaTime);
            }
        }

        public T Get<T>() {
            return (T) _updatables[typeof(T)];
        }

        public void SpawnDebugObject(Vector3 position, float size = 1f) {
            var debug = Object.Instantiate(Resources.Load<GameObject>("_dev/debug/mark"), position, Quaternion.identity);
            debug.transform.localScale = new Vector3(size, size, size);
        }
    }
}