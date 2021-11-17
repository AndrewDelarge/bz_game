using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor.UnityUpperMenu
{
    [ExecuteInEditMode]
    public class BZTools : MonoBehaviour
    {
        public const string ROOT_SCENE_PATH = "Assets/Scenes/_root.unity";

        private static Scene _scene;

        [MenuItem("BZ Tools/Play current scene")]
        static void StartCurrentScene()
        {
            EditorApplication.EnterPlaymode();

            if (_scene != default)
            {
                EditorSceneManager.CloseScene(_scene, false);
                _scene = default;
            }
            
            var scene = SceneManager.GetActiveScene();
            if (scene.buildIndex == 0) {
                Debug.LogWarning("Not playable scene");
                return;
            }
            
            EditorSceneManager.sceneOpened += SceneLoaded;
            var sceneSetups = EditorSceneManager.GetSceneManagerSetup();
            sceneSetups[0].isActive = false;
            var rootScene = EditorSceneManager.OpenScene(ROOT_SCENE_PATH, OpenSceneMode.Additive);
            EditorSceneManager.SetActiveScene(rootScene);
        }

        private static void SceneLoaded(Scene scene, OpenSceneMode openSceneMode)
        {
            var sceneSetups = EditorSceneManager.GetSceneManagerSetup();
            _scene = scene;
            sceneSetups[0].isActive = true;
            EditorSceneManager.sceneOpened -= SceneLoaded;
        }
    }
}