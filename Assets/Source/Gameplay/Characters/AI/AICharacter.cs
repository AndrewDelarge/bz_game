using System;
using System.Collections.Generic;
using game.core;
using game.core.Storage.Data.Character;
using game.gameplay.characters;
using game.Source.Gameplay.Characters.Common;
using UnityEngine;

namespace game.Source.Gameplay.Characters.AI {
	public class AICharacter : MonoBehaviour, ICharacter {
		[SerializeField] private CharacterAnimation _animation;
		[SerializeField] private CharacterMovement _movement;
		[SerializeField] private CharacterAnimData _animData;
		[SerializeField] private CharacterCommonData _data;
		[SerializeField] private Healthable _healthable;
		
		private Dictionary<Type, CharacterState> _states = new() {
			{typeof(AIIdleState), new AIIdleState()},
			{typeof(AIDeadState), new AIDeadState()},
		};

		private BaseStateMachine _mainStateMachine = new ();

		private void Start() {
			game.Core.Get<CharactersManager>().RegisterCharacter(this);
		}

		public void Init() {
			var context = new CharacterContext(_healthable, _movement, _animation, _animData, _movement.transform, _data, _mainStateMachine, _states);
			
			_animation.Init(_animData);

			foreach (var state in _states.Values) {
				state.Init(context);
			}

			_mainStateMachine.ChangeState(_states[typeof(AIIdleState)]);
		}
		
		
		private void Update()
		{
			_mainStateMachine.currentState.HandleState();
		}
	}
}