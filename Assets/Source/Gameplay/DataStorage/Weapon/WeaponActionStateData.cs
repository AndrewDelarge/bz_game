using game.Gameplay.Characters;
using game.Gameplay.Characters.Player;
using UnityEngine;

namespace game.core.storage.Data.Character {
	[CreateAssetMenu(fileName = "Create PlayerActionWeaponEquipIdle", menuName = "GameData/Character/States/PlayerActionWeaponEquipIdle", order = 0)]
	public class WeaponActionStateData : StateData<CharacterState<PlayerActionStateEnum, PlayerCharacterContext>, 
		PlayerActionStateEnum,PlayerCharacterContext> {
		
		private CharacterState<PlayerActionStateEnum, PlayerCharacterContext> _state = new PlayerActionWeaponEquipState();

		public override CharacterState<PlayerActionStateEnum, PlayerCharacterContext> GetState() => _state;
	}
}