using System;
using System.Collections.Generic;
using game.core;
using game.core.Storage.Data.Character;
using game.Gameplay.Characters.Player;
using game.Gameplay.Characters;
using game.Gameplay.Characters.Common;
using UnityEngine;

namespace game.Gameplay.Characters.AI {
	public class AICharacter : MonoBehaviour, ICharacter {
		[SerializeField] private CharacterAnimation _animation;
		[SerializeField] private CharacterMovement _movement;
		[SerializeField] private CharacterAnimationSet _animSet;
		[SerializeField] private CharacterCommonData _data;
		[SerializeField] private Healthable _healthable;
        [SerializeField] private string[] nearMessages = new string[0];
        [SerializeField] private string[] eMessages = new string[0];
		private CharacterStateMachine<CharacterStateEnum, CharacterContext> _mainStateMachine;
		private bool isInited;
		
		public bool isPlayer => false;
		
		public HealthChange<DamageType> GetDamage()
		{
			throw new NotImplementedException();
		}

		private void Start() {
			game.AppCore.Get<CharactersManager>().RegisterCharacter(this);
		}

		public void Init()
		{
			var context = new CharacterContext(_healthable, _movement, _animation, _animSet, _movement.transform, _data);

			_mainStateMachine = new CharacterStateMachine<CharacterStateEnum, CharacterContext>(context,
				new Dictionary<CharacterStateEnum, CharacterState<CharacterStateEnum, CharacterContext>>()
				{
					{CharacterStateEnum.IDLE, new AIIdleState()},
					{CharacterStateEnum.DEAD, new AIDeadState()}
				});

			context.mainStateMachine = _mainStateMachine;
			
			_animation.Init(_animSet);
			
			_mainStateMachine.ChangeState(CharacterStateEnum.IDLE);

			isInited = true;
		}
		
		private void Update() {
			if (isInited == false) {
				return;
			}
			
			_mainStateMachine.HandleState();
		}
        private void OnTriggerEnter(Collider other) {
			var player = other.GetComponent<Player>();
			if (player == null) return;
			player.near = this;
            int messgaeIndex = UnityEngine.Random.Range(0, nearMessages.Length);
			Debug.LogError(nearMessages[messgaeIndex]);
        }

        private void OnTriggerExit(Collider other) {
            var player = other.GetComponent<Player>();
            if (player == null) return;
			if (player.near == this) { player.near = null; }
        }

        public void OnPressE(Player p) {
            int messgaeIndex = UnityEngine.Random.Range(0, eMessages.Length);
            Debug.LogError(eMessages[messgaeIndex]);
        }

    }
}