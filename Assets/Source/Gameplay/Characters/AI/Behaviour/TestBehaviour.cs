using game.core;
using game.Gameplay.Characters.AI.Common;
using game.Gameplay.Characters.Common;

namespace game.Gameplay.Characters.AI.Behaviour
{
    public class TestBehaviour : AIBehaviour {
        private ICharacter _character;
        private const float UPDATE_TICK = 2f;
        private INavigator _navigator;
        private AICharacterControl _control = new ();

        private float _lastUpdate;
        
        public TestBehaviour() {
            _navigator = AppCore.Get<LevelManager>().navigator;
        }

        public override void Init(ICharacter character) {
            _character = character;

            _control.Init(character.controlable);
        }

        public override void Update(float deltaTime) {
            _control.Update(deltaTime);
            
            if (_lastUpdate > 0) {
                _lastUpdate -= deltaTime;
                return;
            }

            if (_control.hasTarget) {
                return;
            }
            
            _lastUpdate = UPDATE_TICK;

            var manager = AppCore.Get<CharactersManager>();

            var path = _navigator.GetPath(_character.currentPosition, manager.player.currentPosition);
            
            if (path == null) {
                return;
            }
            
            _control.FollowPath(path);
        }
    }
}