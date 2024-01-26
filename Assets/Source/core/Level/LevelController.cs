﻿using System;
using System.Collections.Generic;
using game.core.common;
using game.core.InputSystem;
using game.core.storage.Data.Level;
using game.gameplay.control;
using UnityEngine;
using UnityEngine.AI;
using ILogger = game.core.Common.ILogger;
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

    public interface INavigator {
        List<Vector3> GetPath(Vector3 start, Vector3 target);
    }
    
    public class Navigator : INavigator {
        private NavMeshPath _cachedRawPath;
        private List<Vector3> _cachedPath = new List<Vector3>();
        private ILogger _logger;
        public void Init()
        {
            _cachedRawPath = new NavMeshPath();
            _logger = AppCore.Get<ILogger>();
        }
        
        public List<Vector3> GetPath(Vector3 start, Vector3 target)
        {
            _cachedPath = new List<Vector3>();
            if (CalculatePath(start, target) == false) {
                _logger.Log("[Navigator] : Error pathfinding");
                return null;
            }
            
            _logger.Log($"[Navigator] : Path was found #[{_cachedRawPath.status.ToString()}]#");

            for (int i = 0; i < _cachedRawPath.corners.Length - 1; i++)
            {
                Debug.DrawLine(_cachedRawPath.corners[i], _cachedRawPath.corners[i + 1], Color.red, 999f);
            }
            
            foreach (var corner in _cachedRawPath.corners)
            {
                _cachedPath.Add(corner);
            }

            return _cachedPath;
        }

        private bool CalculatePath(Vector3 start, Vector3 target)
        {
            return NavMesh.CalculatePath(start, target, 1, _cachedRawPath);
        }
    }
}