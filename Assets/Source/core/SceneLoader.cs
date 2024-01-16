using System.Collections.Generic;
using game.core.Common;
using game.core.storage.Data.Level;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using ILogger = game.core.Common.ILogger;

namespace game.core
{
    public class SceneLoader {
        private static string DATA_PATH = "Data/Levels";
        private static int DEFAULT_INDEX = -1;
        
        private Scene _root;
        private Scene _currentScene;
        private AsyncOperation _loading;
        private Dictionary<int, LevelData> _levelsData = new ();
        private Whistle<Scene, LevelData> _sceneLoaded = new ();

        public IWhistle<Scene, LevelData> sceneLoaded => _sceneLoaded;
        
        public SceneLoader()
        {
            _root = SceneManager.GetActiveScene();
            SceneManager.sceneLoaded += SceneLoadingComplete;
            var levels = Resources.LoadAll<LevelData>(DATA_PATH);
            
            foreach (var levelData in levels) {
                _levelsData.Add(levelData.sceneId, levelData);
            }
        }
        
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

        private void LoadingComplete(AsyncOperation _)
        {
            _loading.completed -= LoadingComplete;
            _loading = null;
        }

        private void SceneLoadingComplete(Scene scene, LoadSceneMode mode)
        {
            _currentScene = scene;
            var levelData = GetLevelData(scene.buildIndex);

            if (levelData == null) {
                AppCore.Get<ILogger>().Error($"SceneLoader: Error; There isnt config for level with index [{scene.buildIndex}]");
                return;
            }
            
            _sceneLoaded.Dispatch(_currentScene, levelData);
        }

        [CanBeNull]
        private LevelData GetLevelData(int index) {
            if (_levelsData.ContainsKey(index) == false) {
                if (_levelsData.ContainsKey(DEFAULT_INDEX)) {
                    AppCore.Get<ILogger>().Log($"SceneLoader: Loading default scene config");
                    return _levelsData[DEFAULT_INDEX];
                }

                return null;
            }

            return _levelsData[index];
        }

        public void LoadCurrent() {
            var scene = SceneManager.GetActiveScene();
            SceneLoadingComplete(scene, LoadSceneMode.Additive);
        }
    }
}