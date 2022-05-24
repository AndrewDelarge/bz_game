using System;
using System.Collections.Generic;
using game.core.Storage.Data.Character;
using game.gameplay.characters;
using game.Source.Gameplay.Characters.Common;
using UnityEngine;

namespace game.Source.Gameplay.Characters {
	public class PlayerCharacterContext : CharacterContext {
		private BaseStateMachine _actionStateMachine;
		protected Dictionary<Type, CharacterState> _actionStates;
		
		public Camera camera;
		public IReadOnlyDictionary<Type, CharacterState> actionStates => _actionStates;
		public BaseStateMachine actionStateMachine => _actionStateMachine;
		public new PlayerCharacterCommonData data => (PlayerCharacterCommonData) _data;

		public PlayerCharacterContext(Healthable healthable, CharacterMovement movement, CharacterAnimation animation, CharacterAnimData animationData, PlayerCharacterCommonData data, Transform transform, BaseStateMachine actionStateMachine, BaseStateMachine mainStateMachine, Dictionary<Type, CharacterState> states, Dictionary<Type, CharacterState> actionStates) : base(healthable, movement, animation, animationData, transform, data, mainStateMachine, states) {
			_data = data;
			_actionStateMachine = actionStateMachine;
			_actionStates = actionStates;
		}
	}
}