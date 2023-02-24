using System;
using System.Collections.Generic;
using game.core.storage.Data.Character;
using game.core.Storage.Data.Character;
using game.Gameplay.Characters;
using game.Gameplay.Characters.Player;
using NaughtyAttributes;
using UnityEngine;

namespace game.core.storage.Data.Equipment {
	public abstract class EquipmentData : ScriptableObject {
		[SerializeField] private CharacterAnimationSet _animationSet;

		[SerializeField] private List<State<CharacterState<CharacterStateEnum>, CharacterStateEnum>> _statesOverrides;
		[SerializeField] private List<State<CharacterState<PlayerActionStateEnum>, PlayerActionStateEnum>> _actionStatesOverrides;
		
		public CharacterAnimationSet animationSet => _animationSet;
		public IReadOnlyList<State<CharacterState<CharacterStateEnum>, CharacterStateEnum>> statesOverrides => _statesOverrides;
		public IReadOnlyList<State<CharacterState<PlayerActionStateEnum>, PlayerActionStateEnum>> actionStatesOverrides => _actionStatesOverrides;
	}
}