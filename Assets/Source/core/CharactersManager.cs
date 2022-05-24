using System.Collections.Generic;
using game.core.common;
using game.Source.Core.Common;
using game.Source.Gameplay.Characters;

namespace game.core {
	public class CharactersManager : ICoreManager, IInitalizeable {
		private List<ICharacter> _characters = new List<ICharacter>();
		
		public void Init() {
			
		}

		public void RegisterCharacter(ICharacter character) {
			character.Init();
			_characters.Add(character);
		}

	}
}