using game.Gameplay.Characters;
using game.Gameplay.Characters.Player;
using UnityEngine;

namespace game.core.storage.Data.Character {
	[CreateAssetMenu(fileName = "Create PlayerActionWeaponEquipIdle", menuName = "GameData/Character/States/PlayerActionWeaponEquipIdle", order = 0)]
	public class WeaponActionState : State<CharacterState<PlayerActionStateEnum>, PlayerActionStateEnum> {
		
		private CharacterState<PlayerActionStateEnum> _state = new PlayerActionWeaponEquipIdle();

		public override CharacterState<PlayerActionStateEnum> GetState() => _state;
	}
}