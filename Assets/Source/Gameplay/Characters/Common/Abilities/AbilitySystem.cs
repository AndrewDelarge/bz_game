using System.Collections.Generic;
using game.core.Common;
using game.core.storage.Data.Abilities;

namespace game.Gameplay.Characters.Common.Abilities {
	public class AbilitySystem {
		private List<IAbility> _abilities = new List<IAbility>();
		private Whistle<IAbility> _onUse = new Whistle<IAbility>();

		public IWhistle<IAbility> onUseAbility => _onUse;
		public void Init(ICharacter character, List<AbilityData> _abilitiesData) {
			foreach (var data in _abilitiesData) {
				var ability = data.ability.value;
				
				ability.Init(data);
				ability.SetCharacter(character);
				ability.onUse.Add(_onUse.Dispatch);
				_abilities.Add(ability);
			}
		}

		public void Update(float delta) {
			foreach (var ability in _abilities) {
				if (ability.isCooldown && ability.isUsing) {
					ability.Update(delta);
				}
			}
		}

		// TODO: temp 
		public IAbility GetBestFreeAbility() {
			IAbility bestAbility = null;
			var longestCooldown = -1f;
			foreach (var ability in _abilities) {
				if (ability.isCooldown || ability.isUsing) {
					continue;
				}

				if (ability.cooldown > longestCooldown) {
					bestAbility = ability;
					longestCooldown = ability.cooldown;
				}
			}

			return bestAbility;
		}
	}
}