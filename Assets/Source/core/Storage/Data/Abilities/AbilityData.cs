using game.core.Storage.Data.Character;
using game.Gameplay.Characters.AI.Behaviour;
using game.Gameplay.Characters.Common.Abilities;
using UnityEngine;

namespace game.core.storage.Data.Abilities {
	[CreateAssetMenu(menuName = "GameData/Abilities/Create new ability data", fileName = "AbilityData", order = 0)]
	public class AbilityData : ScriptableObject {
		[SerializeField] private float _cooldown;
		[SerializeField] private float _abilityTime;

		[SerializeField] private AnimationData<CharacterAnimationEnum> _animation;
		
		[SerializeField] private LogicReference<IAbility> _ability;
		[SerializeField] private LogicReference<AbilityBehaviourState> _specialBehaviour;

		public float cooldown => _cooldown;
		public float abilityTime => _abilityTime;

		public AnimationData<CharacterAnimationEnum> animation => _animation;

		public LogicReference<IAbility> ability => _ability;

		public LogicReference<AbilityBehaviourState> specialBehaviour => _specialBehaviour;
	}
}