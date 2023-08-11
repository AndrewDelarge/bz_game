using game.core.common;
using game.core.storage.Data.Abilities;
using game.сore.Common;
using UnityEngine;

namespace game.Gameplay.Characters.Common {
	public class CameraManager : ICoreManager, IInitalizeable {
		private const string DATA_PATH = "Data/CameraData";
		
		private CameraData _data;


		public void Init() {
			_data = Resources.Load<CameraData>(DATA_PATH);
		}

		public Vector2 GetScreenResolutionDelta() {
			return new((float) _data.texture.width / Screen.width, (float) _data.texture.height / Screen.height);
		}
	}
}