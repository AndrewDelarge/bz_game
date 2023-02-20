using System;
using System.Collections.Generic;
using game.core;
using game.core.Storage.Data.Character;
using game.gameplay.characters;
using game.Gameplay.Characters.Common;
using UnityEngine;

namespace game.Gameplay.Characters.AI {
	public class AICharacter : MonoBehaviour, ICharacter {
		[SerializeField] private CharacterAnimation _animation;
		[SerializeField] private CharacterMovement _movement;
		[SerializeField] private CharacterAnimData _animData;
		[SerializeField] private CharacterCommonData _data;
		[SerializeField] private Healthable _healthable;

		private CharacterStateMachine<CharacterStateEnum> _mainStateMachine;
		private bool isInited;
		
		public bool isPlayer => false;

		private void Start() {
			game.AppCore.Get<CharactersManager>().RegisterCharacter(this);
		}

		public void Init()
		{
			_mainStateMachine = new CharacterStateMachine<CharacterStateEnum>(
				new Dictionary<CharacterStateEnum, CharacterState<CharacterStateEnum>>()
				{
					{CharacterStateEnum.IDLE, new AIIdleState()},
					{CharacterStateEnum.DEAD, new AIDeadState()}
				});
			
			var context = new CharacterContext(_healthable, _movement, _animation, _animData, _movement.transform, _data, _mainStateMachine);
			
			_animation.Init(_animData);

			foreach (var state in _mainStateMachine.states.Values) {
				state.Init(context);
			}

			_mainStateMachine.ChangeState(CharacterStateEnum.IDLE);

			isInited = true;
		}
		
		private void Update()
		{
			if (isInited == false) {
				return;
			}
			
			_mainStateMachine.currentState.HandleState();
		}
	}
}