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
		[SerializeField] private CharacterEquiperType _equiperType;
		public CharacterAnimationSet animationSet => _animationSet;
		public IReadOnlyList<StateData<CharacterState<CharacterStateEnum, PlayerCharacterContext>, CharacterStateEnum,PlayerCharacterContext>> statesOverrides => _statesOverrides;
		public IReadOnlyList<StateData<CharacterState<PlayerActionStateEnum, PlayerCharacterContext>, PlayerActionStateEnum,PlayerCharacterContext>> actionStatesOverrides => _actionStatesOverrides;
		public CharacterEquiperType equiperType => _equiperType;
		
		// TODO Придумать как инвертировать зависимость (нужна фабрика)
		public abstract IEquipment CreateModel();
	}
}