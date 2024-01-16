using game.core.common;
using game.core.storage.Data.Level;
using game.Gameplay.Weapon;
using game.сore.Common;
using UnityEngine.SceneManagement;

namespace game.core {
	public class LevelManager : ICoreManager, IInitalizeable {
		private SceneLoader _sceneLoader;
		private LevelController _levelController;
		private CharactersController _characterController;

		public LevelController levelController => _levelController;
		public CharactersController characterController => _characterController;

		public void Init() {
			_characterController = new CharactersController();
			_sceneLoader = new SceneLoader();
            
			_sceneLoader.sceneLoaded.Add(LoadHandler);
		}
		
		public void Load(int index) {
			_sceneLoader.LoadScene(index);
		}
		
		public void LoadCurrent() {
			_sceneLoader.LoadCurrent();
		}

		public void Update(float deltaTime) {
			_levelController?.Update(deltaTime);
		}
		
		private void LoadHandler(Scene scene, LevelData data) {
			_levelController = new LevelController();
			_levelController.Init(data);
			_levelController.Add(new ProjectileController());
			_levelController.Add(_characterController);
		}
	}
}