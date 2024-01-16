using System;
using System.Collections.Generic;
using game.Gameplay.Characters.Common;

namespace game.core {

	public abstract class AIBehaviour
	{
		public abstract void Init(ICharacter character);

		public abstract void Update(float deltaTime);
	}
	
	
	public class AIManager {
		private List<AIBehaviour> _behaviours = new List<AIBehaviour>();
		
		public void Add(ICharacter character) {
			if (character.behaviour != null) {
				_behaviours.Add(character.behaviour);
			}
		}

		public void Update(float deltaTime) {
			foreach (var behaviour in _behaviours) {
				behaviour.Update(deltaTime);
			}
		}
		
	}
	public class CharactersController : IUpdatable {
		private List<ICharacter> _characters = new List<ICharacter>();

		private ICharacter _player;
		private AIManager _manager = new AIManager();

		public ICharacter player => _player;

		public void RegisterCharacter(ICharacter character) {
			character.Init();
			
			if (character.isPlayer) {
				AddPlayer(character);
				return;
			}
			
			_characters.Add(character);
			_manager.Add(character);
		}

		public void AddPlayer(ICharacter player) {
			if (_player != null) {
				throw new CharacterManagerException("Player already added");
			}
			
			_player = player;
		}

		public void Update(float deltaTime) {
			_manager.Update(deltaTime);
		}
	}

	public class CharacterManagerException : Exception {
		public CharacterManagerException(string message) : base(message) {}
	}
}