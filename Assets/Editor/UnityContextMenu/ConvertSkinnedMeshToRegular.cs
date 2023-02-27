using UnityEditor;
using UnityEngine;

namespace Editor.UnityContextMenu {
	[InitializeOnLoad]
	public class ConvertSkinnedMeshToRegular {
		[MenuItem("Assets/Tools/Renderer/Convert Skinned To Regular")]
		static void Convert(MenuCommand command) {
			var path = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]);
            
			var gameObject = PrefabUtility.LoadPrefabContents(path);

			var skinnedMeshRenderer = gameObject.GetComponent<SkinnedMeshRenderer>();

			if (skinnedMeshRenderer == null) {
				Debug.LogError($"Skinned mesh not found in {path}");
				return;
			}
			
			var meshRenderer = gameObject.AddComponent<MeshRenderer>();
			var meshFilter = gameObject.AddComponent<MeshFilter>();

			meshFilter.sharedMesh = skinnedMeshRenderer.sharedMesh;
			meshRenderer.sharedMaterials = skinnedMeshRenderer.sharedMaterials;
            
			Object.DestroyImmediate(skinnedMeshRenderer);

			PrefabUtility.SaveAsPrefabAsset(gameObject, path);
            
			PrefabUtility.UnloadPrefabContents(gameObject);
			
			Debug.Log($"Renderer was converted successful <{path}>");
		}
	}
}