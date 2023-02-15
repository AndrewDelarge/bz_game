using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Editor.UnityUpperMenu
{
    [ExecuteInEditMode]
    [InitializeOnLoad]
    public class BZTools : MonoBehaviour
    {
        public const string ROOT_SCENE_PATH = "Assets/Scenes/_root.unity";
        
        private static readonly Action _exitPlaymode;
        
        static BZTools() {
            EditorApplication.playModeStateChanged += ChangePlayModeHandler;
            _exitPlaymode += ClearRootScene;
        }
        
        [MenuItem("BZ Tools/Play current scene")]
        static void StartCurrentScene()
        {
            EditorApplication.EnterPlaymode();

            var rootScene = EditorSceneManager.OpenScene(ROOT_SCENE_PATH, OpenSceneMode.Additive);
            EditorSceneManager.SetActiveScene(rootScene);
        }

        private static void ChangePlayModeHandler(PlayModeStateChange obj) {
            if (obj == PlayModeStateChange.EnteredEditMode ) 
            {
                _exitPlaymode.Invoke();
            }

            // "mb later" feature  
            // if (obj == PlayModeStateChange.ExitingEditMode) {
            //     _rootScene = EditorSceneManager.OpenScene(ROOT_SCENE_PATH, OpenSceneMode.Additive);
            //     EditorSceneManager.SetActiveScene(_rootScene);
            // }
        }
        
        private static void ClearRootScene() {
            var scene = EditorSceneManager.GetSceneByPath(ROOT_SCENE_PATH);
            EditorSceneManager.CloseScene(scene, removeScene:true);
        }
    }
}