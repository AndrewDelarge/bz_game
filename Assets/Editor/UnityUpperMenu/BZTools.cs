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
        private static Scene _rootScene;

        [MenuItem("BZ Tools/Play current scene")]
        static void StartCurrentScene()
        {
            EditorApplication.EnterPlaymode();

            var rootScene = EditorSceneManager.OpenScene(ROOT_SCENE_PATH, OpenSceneMode.Additive);
            EditorSceneManager.SetActiveScene(rootScene);
        }
    }
}