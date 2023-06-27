using game.core;
using game.Gameplay.Characters.Common;
using UnityEngine;

namespace game.Gameplay.Characters.AI.Behaviour
{
    public class TestBehaviour : AIBehaviour {
        private ICharacter _character;
        private const float UPDATE_TICK = 2f;
        private INavigator _navigator;

        private float _lastUpdate;
        
        public TestBehaviour() {
            _navigator = AppCore.Get<LevelManager>().navigator;
        }

        public override void Init(ICharacter character) {
            _character = character;
        }

        public override void Update(float deltaTime) {
            if (_lastUpdate > 0) {
                _lastUpdate -= deltaTime;
                return;
            }

            _lastUpdate = UPDATE_TICK;

            var manager = AppCore.Get<CharactersManager>();

            _navigator.GetPath(_character.currentPosition, manager.player.currentPosition);
        }
    }
}