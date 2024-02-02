using System;
using System.Collections.Generic;
using game.core.common;
using game.core.InputSystem;
using game.core.storage.Data.Level;
using game.Gameplay;
using game.Gameplay.Characters.Player;
using game.gameplay.control;
using UnityEngine;
using Object = UnityEngine.Object;

namespace game.core.level
{
    public class LevelController
    {
        private Dictionary<Type, IUpdatable> _updatables = new Dictionary<Type, IUpdatable>();
        private Navigator _navigator;
        private PlayerCharacter _player;
        private InputListener _inputListener;
        private GameCamera _camera;

        public INavigator navigator => _navigator;
        
        public void Init(LevelData data)
        {
            _inputListener = Object.Instantiate(Resources.Load<InputListener>("InputListener"));
            _player = Object.Instantiate(data.playerCharacter, Vector3.zero, Quaternion.identity);
            _camera = Object.Instantiate(data.camera, Vector3.zero, Quaternion.identity);

            _player.SetCamera(_camera.camera);
            _camera.SetTarget(_player.currentTransform);
            AppCore.Get<CameraManager>().AddCamera(_camera);
            AppCore.Get<LevelManager>().characterController.RegisterCharacter(_player);
            AppCore.Get<IInputManager>().RegisterControlable(_player);

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

        public void Dispose() {
            AppCore.Get<IInputManager>().RemoveControlable(_player);

            Object.Destroy(_player.gameObject);
            Object.Destroy(_camera.gameObject);
            Object.Destroy(_inputListener.gameObject);
            
            _updatables.Clear();
        }
    }
}