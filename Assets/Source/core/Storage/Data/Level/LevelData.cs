using Cinemachine;
using game.Gameplay;
using game.Gameplay.Characters.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace game.core.storage.Data.Level {
	[CreateAssetMenu(menuName = "GameData/Level/LevelData", fileName = "LevelData", order = 0)]
	public class LevelData : ScriptableObject {
		[SerializeField] private GameCamera _camera;
		[SerializeField] private PlayerCharacter _playerCharacter;
		[SerializeField] private int _sceneId;

		public int sceneId => _sceneId;
		public GameCamera camera => _camera;
		public PlayerCharacter playerCharacter => _playerCharacter;
	}
}