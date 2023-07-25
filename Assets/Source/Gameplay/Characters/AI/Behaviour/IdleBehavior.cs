using game.core;
using game.Gameplay.Characters.Common;

namespace game.Gameplay.Characters.AI.Behaviour {
	public class IdleBehavior : AIBehaviour {
		private BaseBehaviourState _state = new IdleBehaviourState();
		public override void Init(ICharacter character) { }

		public override void Update(float deltaTime) {
			_state.HandleState(deltaTime);
		}
	}
}