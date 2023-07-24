using game.Gameplay.Characters.Common.Abilities;
using UnityEngine;

namespace game.core.storage.Data.Abilities {
	[CreateAssetMenu(menuName = "GameData/Abilities/Logic/Create TestAbilityLogic", fileName = "TestAbilityLogic", order = 0)]
	public class TestAbilityLogicReference : LogicReference<IAbility> {
		public override IAbility value => new TestAbility();
	}
}