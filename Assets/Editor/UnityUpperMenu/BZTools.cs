using System;
using game;
using game.core.level;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Editor.UnityUpperMenu
{
    [ExecuteInEditMode]
    [InitializeOnLoad]
    public class BZTools : MonoBehaviour {
        public const string ROOT_SCENE_PATH = "Assets/Scenes/_root.unity";
        
        private static readonly Action _exitPlaymode;
        private static Action _enterPlaymode;
        private static bool _test;
        
        static BZTools() {
            EditorApplication.playModeStateChanged += ChangePlayModeHandler;
            _exitPlaymode += ClearRootScene;
        }
        
        [MenuItem("BZ Tools/Play current scene")]
        static void StartCurrentScene()
        {
            EditorApplication.EnterPlaymode();

            EditorSceneManager.OpenScene(ROOT_SCENE_PATH, OpenSceneMode.Additive);
        }

        [MenuItem("BZ Tools/Restart current scene")]
        static void RestartCurrentScene() {
            if (EditorApplication.isPlaying == false) {
                Debug.LogError("Игру сначала запусти, еблан");
                return;
            }
            
            AppCore.Get<LevelManager>().ReloadCurrent();
        }

        private static void ChangePlayModeHandler(PlayModeStateChange obj) {
            if (obj == PlayModeStateChange.EnteredEditMode ) 
            {
                _exitPlaymode.Invoke();
            }

            if (obj == PlayModeStateChange.EnteredPlayMode) {
                var levelManager = AppCore.Get<LevelManager>();
                
                levelManager?.LoadCurrent();
            }
        }
        
        private static void ClearRootScene() {
            var scene = EditorSceneManager.GetSceneByPath(ROOT_SCENE_PATH);
            EditorSceneManager.CloseScene(scene, removeScene:true);
        }
    }
}