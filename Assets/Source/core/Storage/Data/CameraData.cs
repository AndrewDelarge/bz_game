using UnityEngine;

namespace game.core.storage.Data.Abilities {
	[CreateAssetMenu(menuName = "GameData/CameraData", fileName = "CameraData", order = 0)]
	public class CameraData : ScriptableObject {
		[SerializeField] private RenderTexture _texture;

		public RenderTexture texture => _texture;
	}
}