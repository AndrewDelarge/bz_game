using System;
using System.Collections.Generic;
using game.core.common;
using game.Gameplay.Characters;
using game.Gameplay.Characters.Common;
using game.сore.Common;
using game.Gameplay.Characters.Player;

namespace game.core {
	public class AIManager {
		
	}
	public class CharactersManager : ICoreManager, IInitalizeable {
		private List<ICharacter> _characters = new List<ICharacter>();

		private ICharacter _player;
		public void Init() {
			
		}

		public void RegisterCharacter(ICharacter character) {
			character.Init();
			
			if (character.isPlayer) {
				AddPlayer(character);
				return;
			}
			
			_characters.Add(character);
		}

		public void AddPlayer(ICharacter player) {
			if (_player != null) {
				throw new CharacterManagerException("Player already added");
			}

			_player = player;
		}
	}

	public class CharacterManagerException : Exception {
		public CharacterManagerException(string message) : base(message) {}
	}
}