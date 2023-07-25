using game.core.Common;
using game.core.InputSystem;
using game.core.storage.Data.Abilities;
using game.Gameplay.Characters.AI.Behaviour;

namespace game.Gameplay.Characters.Common.Abilities {
	public class TestAbility : IAbility {
		private AbilityData _data;
		private AbilityBehaviourState _behaviourState;
		private Whistle<IAbility> _onUse = new Whistle<IAbility>();
		private ICharacter _character;
		private ICharacter _target;
		
		private bool _isUsing;
		private float _currentCooldown;
		
		public float cooldown => _currentCooldown;
		public float abilityTime => _data.abilityTime;
		public bool isCooldown => _currentCooldown > 0;
		public bool isUsing => _isUsing;
		public AbilityBehaviourState behaviourState => _behaviourState;
		public IWhistle<IAbility> onUse => _onUse;
		
		public void Init(AbilityData data) {
			_data = data;
			// todo: refactor this? 
			_behaviourState = _data.specialBehaviour.value;
			_behaviourState.SetAbility(this);
		}

		public void SetCharacter(ICharacter character) {
			_character = character;
		}

		public void SetTarget(ICharacter target) {
			_target = target;
		}

		public void Update(float delta) {
			if (_currentCooldown > 0) {
				_currentCooldown -= delta;
				return;
			}

			_isUsing = false;
		}

		public void Use() {
			_character.animator.PlayAnimation(_data.animation.clip);
			_target.healthable.TakeDamage(_character.GetDamage());
			
			_isUsing = true;
			_currentCooldown = _data.cooldown;
			_onUse.Dispatch(this);
		}

		public void Interrupt() {
			throw new System.NotImplementedException();
		}

		public void HandleInput(InputData data) {
			
		}
	}
}