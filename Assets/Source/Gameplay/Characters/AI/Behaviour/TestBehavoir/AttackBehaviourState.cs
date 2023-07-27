using game.core.InputSystem;
using game.Gameplay.Characters.Common.Abilities;
using game.Source.core.Common;
using UnityEngine;

namespace game.Gameplay.Characters.AI.Behaviour {
	public class AttackBehaviourState : BaseBehaviourState {
		private const int ATTACK_RANGE = 1;
		private IAbility _currentAbility;
		public override BehaviourState type => BehaviourState.ATTACK;
		
		public override void HandleState(float deltaTime) {
			if (_context.target == null) {
				_context.stateMachine.ChangeState(BehaviourState.TARGET_SEARCHING);
				return;
			}
			if (Vector3.Distance(_context.character.currentPosition, _context.target.currentPosition) > ATTACK_RANGE) {
				_context.stateMachine.ChangeState(BehaviourState.TARGET_FOLLOW);
				return;
			}
			
			var ability = _context.character.abilitySystem.GetBestFreeAbility();

			if (ability == null) {
				return;
			}
			
			if (ability.behaviourState != null) {
				_context.stateMachine.ChangeState(ability.behaviourState);
			} else {
				_currentAbility = ability;
				UseAbilityFallback(ability);
			}
		}

		public override void HandleInput(InputData data) {
			if (_currentAbility != null) {
				_currentAbility.HandleInput(data);
			}
		}

		public override void Enter() {
			_context.target?.healthable.die.Add(OnTargetDie);
		}

		public override void Exit() {
			_context.target?.healthable.die.Remove(OnTargetDie);
		}

		private void UseAbilityFallback(IAbility ability) {
			ability.SetTarget(_context.target);
			ability.Use();
			AppCore.Get<GameTimer>().SetTimeout(ability.abilityTime, OnAbilityEnd);
		}

		private void OnAbilityEnd() {
			_currentAbility = null;
		}

		private void OnTargetDie() {
			_context.target = null;
		}
	}
}