using System;
using System.Collections.Generic;
using game.core.Storage.Data.Character;
using game.gameplay.characters;
using game.Gameplay.Characters.Player.Common;
using UnityEngine;

namespace game.Gameplay.Characters.Player
{
	public class PlayerCharacterContext : CharacterContext {
		private CharacterStateMachine<PlayerActionStateEnum> _actionStateMachine;
		
		public Camera camera;
		public CharacterStateMachine<PlayerActionStateEnum> actionStateMachine => _actionStateMachine;
		public new PlayerCharacterCommonData data => (PlayerCharacterCommonData) _data;

		public PlayerCharacterContext(Healthable healthable, CharacterMovement movement, CharacterAnimation animation, 
			CharacterAnimData animationData, PlayerCharacterCommonData data, Transform transform, 
			CharacterStateMachine<PlayerActionStateEnum> actionStateMachine, CharacterStateMachine<CharacterStateEnum> mainStateMachine) 
			: base(healthable, movement, animation, animationData, transform, data, mainStateMachine) {
			_data = data;
			_actionStateMachine = actionStateMachine;
		}
	}
}