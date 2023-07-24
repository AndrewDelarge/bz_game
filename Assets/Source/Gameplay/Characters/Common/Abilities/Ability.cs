using game.core.storage.Data.Abilities;

namespace game.Gameplay.Characters.Common.Abilities {
	public interface IAbility {
		bool isReady { get; }
		void Init(AbilityData data);
		void Update(float delta);
		void Use();
		void Interrupt();
	}
	
	
	
	public class TestAbility : IAbility {
		public bool isReady { get; }

		public void Init(AbilityData data) {
			throw new System.NotImplementedException();
		}

		public void Update(float delta) {
			throw new System.NotImplementedException();
		}

		public void Use() {
			throw new System.NotImplementedException();
		}

		public void Interrupt() {
			throw new System.NotImplementedException();
		}
	}
}