using System;
using System.Collections.Generic;
using game.core;
using game.core.InputSystem;
using game.core.InputSystem.Interfaces;
using game.core.level;
using game.core.Storage.Data.Character;
using game.Gameplay.Characters.Common;
using game.Gameplay.Characters.Common.Abilities;
using UnityEngine;

namespace game.Gameplay.Characters.AI {
	public class AICharacter : MonoBehaviour, ICharacter, IControlable {
		[SerializeField] private CharacterAnimation _animation;
		[SerializeField] private CharacterMovement _movement;
		[SerializeField] private AICharacterData _data;
		[SerializeField] private Healthable _healthable;
        [SerializeField] private string[] nearMessages = new string[0];
        [SerializeField] private string[] eMessages = new string[0];
		private CharacterStateMachine<CharacterStateEnum, CharacterContext> _mainStateMachine;
		private bool isInited;
		private InputData _inputData;
		private AIBehaviour _behaviour;
		private AbilitySystem _abilitySystem = new AbilitySystem();
		
		public bool isPlayer => false;
		public bool canChangeState => _mainStateMachine.canExitState;

		public Vector3 currentPosition => transform.position;
		public Transform currentTransform => transform;

		public AICharacterData data => _data;

		public IControlable controlable => this;

		public AIBehaviour behaviour => _behaviour ??= GetNewBehavior();
		public AbilitySystem abilitySystem => _abilitySystem;
		public CharacterAnimation animator => _animation;
		public Healthable healthable => _healthable;

		private Dictionary<CharacterStateEnum, CharacterState<CharacterStateEnum, CharacterContext>> _states = new Dictionary<CharacterStateEnum, CharacterState<CharacterStateEnum, CharacterContext>>() {
			{CharacterStateEnum.IDLE, new AIIdleState()},
			{CharacterStateEnum.WALK, new AIWalkState()},
			{CharacterStateEnum.DEAD, new AIDeadState()},
			{CharacterStateEnum.ABILITY, new AIUseAbilityState()}
		};
		
		public HealthChange<DamageType> GetDamage() {
			return new HealthChange<DamageType>(25, DamageType.PHYSICS);
		}

		private void Start() {
			if (isInited == false) {
				AppCore.Get<LevelManager>().characterController.RegisterCharacter(this);
			}
		}

		public void Init()
		{
			var context = new CharacterContext(_healthable, _movement, _animation, data.animationSet, _movement.transform, _data);
			_mainStateMachine = new CharacterStateMachine<CharacterStateEnum, CharacterContext>(context, _states);

			context.mainStateMachine = _mainStateMachine;
			
			_animation.Init(data.animationSet);
			
			_mainStateMachine.ChangeState(CharacterStateEnum.IDLE);
			
			_abilitySystem.Init(this, _data.abilities);
			_abilitySystem.onUseAbility.Add(OnUseAbilityHandler);
			isInited = true;
		}

		private void OnUseAbilityHandler(IAbility ability) {
			var state = (AIUseAbilityState) _states[CharacterStateEnum.ABILITY];
			state.SetAbility(ability);
			
			_mainStateMachine.ChangeState(CharacterStateEnum.ABILITY);
		}

		private void Update() {
			if (isInited == false) {
				return;
			}

			if (_inputData != null) {
				_mainStateMachine.HandleInput(_inputData);
			}
			
			_mainStateMachine.HandleState(Time.deltaTime);
			_abilitySystem.Update(Time.deltaTime);
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

        public bool isListen { get; }
        public void OnDataUpdate(InputData data) {
	        _inputData = data;
        }

        private void OnDrawGizmos() {
	        if (_inputData == null) {
		        return;
	        }
	        
	        Gizmos.color = Color.magenta;
	        Gizmos.DrawLine(transform.position, transform.position + (Vector3) (_inputData.move.value * 10));
        }
        
        private AIBehaviour GetNewBehavior() {
	        if (data.behaviourData != null) {
		        var behaviour = data.behaviourData.GetBehaviour();
		        behaviour.Init(this);
		        return behaviour;
	        }

	        return null;
        }
	}
}