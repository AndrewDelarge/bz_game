using game.Gameplay.Characters.AI.Behaviour;
using UnityEngine;

namespace game.core.storage.Data.Abilities {
	[CreateAssetMenu(menuName = "GameData/Abilities/Logic/Create TestBehaviourStateLogicReference", fileName = "TestBehaviourStateLogicReference", order = 0)]
	public class TestBehaviourStateLogicReference : LogicReference<AbilityBehaviourState> {
		public override AbilityBehaviourState value => new TestSpecialAbilityBehaviourState();
	}
}