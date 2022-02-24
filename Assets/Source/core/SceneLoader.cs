using System;
using game.core.common;
using game.Source.Core.Common;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace game.core
{
    public class SceneLoader : IInitalizeable, ICoreManager
    {
        public event Action<Scene> sceneLoaded;
        
        private Scene _root;
        private Scene _currentScene;
        private AsyncOperation _loading;
        
        public void Init()
        {
            _root = SceneManager.GetActiveScene();
            SceneManager.sceneLoaded += SceneLoadingComplete;
        }
        
        // TODO Make scene dictionary\enum
        public void LoadScene(int id)
        {
            if (_loading != null)
            {
                Debug.LogError($"Scene loading in progress");
                return;
            }
            if (_root.buildIndex == id)
                return;

            if (_currentScene != default)
            {
                SceneManager.UnloadSceneAsync(_currentScene);
                _currentScene = default;
            }

            _loading = SceneManager.LoadSceneAsync(id, LoadSceneMode.Additive);
            
            _loading.completed += LoadingComplete;
        }

        private void LoadingComplete(AsyncOperation obj)
        {
            _loading.completed -= LoadingComplete;
            _loading = null;
        }

        private void SceneLoadingComplete(Scene scene, LoadSceneMode mode)
        {
            _currentScene = scene;
            sceneLoaded?.Invoke(_currentScene);
        }
    }
}