using System;
using System.Collections.Generic;
using game.core;
using game.core.InputSystem;
using game.core.InputSystem.Interfaces;
using game.core.Storage.Data.Character;
using game.Gameplay.Characters.Common;
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
		public bool isPlayer => false;

		public Vector3 currentPosition => transform.position;
		public Transform currentTransform => transform;

		public AICharacterData data => _data;

		public IControlable controlable => this;

		public AIBehaviour behaviour => _behaviour ??= GetNewBehavior();

		public HealthChange<DamageType> GetDamage()
		{
			throw new NotImplementedException();
		}

		private void Start() {
			game.AppCore.Get<CharactersManager>().RegisterCharacter(this);
		}

		public void Init()
		{
			var context = new CharacterContext(_healthable, _movement, _animation, data.animationSet, _movement.transform, _data);

			_mainStateMachine = new CharacterStateMachine<CharacterStateEnum, CharacterContext>(context,
				new Dictionary<CharacterStateEnum, CharacterState<CharacterStateEnum, CharacterContext>>()
				{
					{CharacterStateEnum.IDLE, new AIIdleState()},
					{CharacterStateEnum.WALK, new AIWalkState()},
					{CharacterStateEnum.DEAD, new AIDeadState()}
				});

			context.mainStateMachine = _mainStateMachine;
			
			_animation.Init(data.animationSet);
			
			_mainStateMachine.ChangeState(CharacterStateEnum.IDLE);

			isInited = true;
		}
		
		private void Update() {
			if (isInited == false) {
				return;
			}

			if (_inputData != null) {
				_mainStateMachine.HandleInput(_inputData);
			}
			
			_mainStateMachine.HandleState(Time.deltaTime);
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