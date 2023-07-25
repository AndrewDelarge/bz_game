using game.core.Common;
using game.core.InputSystem;
using game.core.storage.Data.Abilities;
using game.Gameplay.Characters.AI.Behaviour;

namespace game.Gameplay.Characters.Common.Abilities {
	public interface IAbility {
		float cooldown { get; }
		float abilityTime { get; }
		bool isCooldown { get; }
		bool isUsing { get; }
		AbilityBehaviourState behaviourState { get; }
		IWhistle<IAbility> onUse { get; }
		void Init(AbilityData data);
		void Update(float delta);
		void Use();
		void Interrupt();
		void HandleInput(InputData data);
		void SetCharacter(ICharacter character);
		void SetTarget(ICharacter target);
	}
}