using System;
using System.Collections.Generic;
using game.core.Storage.Data.Character;
using game.gameplay.characters;
using game.Source.Gameplay.Characters.Common;
using UnityEngine;

namespace game.Source.Gameplay.Characters {
	public class PlayerCharacterContext : CharacterContext {
		private CharacterStateMachine<PlayerActionState> _actionStateMachine;
		
		public Camera camera;
		public CharacterStateMachine<PlayerActionState> actionStateMachine => _actionStateMachine;
		public new PlayerCharacterCommonData data => (PlayerCharacterCommonData) _data;

		public PlayerCharacterContext(Healthable healthable, CharacterMovement movement, CharacterAnimation animation, 
			CharacterAnimData animationData, PlayerCharacterCommonData data, Transform transform, 
			CharacterStateMachine<PlayerActionState> actionStateMachine, CharacterStateMachine<CharacterStateEnum> mainStateMachine) 
			: base(healthable, movement, animation, animationData, transform, data, mainStateMachine) {
			_data = data;
			_actionStateMachine = actionStateMachine;
		}
	}
}