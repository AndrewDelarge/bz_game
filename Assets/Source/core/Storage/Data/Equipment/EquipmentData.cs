using System.Collections.Generic;
using game.core.storage.Data.Character;
using game.core.Storage.Data.Character;
using game.core.storage.Data.Models;
using game.Gameplay.Characters;
using game.Gameplay.Characters.Player;
using UnityEngine;

namespace game.core.storage.Data.Equipment {
	public abstract class EquipmentData : ScriptableObject {
		[SerializeField] private CharacterAnimationSet _animationSet;

		[SerializeField] private List<StateData<CharacterState<CharacterStateEnum, PlayerCharacterContext>, CharacterStateEnum,PlayerCharacterContext>> _statesOverrides;
		[SerializeField] private List<StateData<CharacterState<PlayerActionStateEnum, PlayerCharacterContext>, PlayerActionStateEnum,PlayerCharacterContext>> _actionStatesOverrides;
		
		public CharacterAnimationSet animationSet => _animationSet;
		public IReadOnlyList<StateData<CharacterState<CharacterStateEnum, PlayerCharacterContext>, CharacterStateEnum,PlayerCharacterContext>> statesOverrides => _statesOverrides;
		public IReadOnlyList<StateData<CharacterState<PlayerActionStateEnum, PlayerCharacterContext>, PlayerActionStateEnum,PlayerCharacterContext>> actionStatesOverrides => _actionStatesOverrides;
		
		// TODO Придумать как инвертировать зависимость 
		public abstract EquipmentModel CreateModel();
	}
}