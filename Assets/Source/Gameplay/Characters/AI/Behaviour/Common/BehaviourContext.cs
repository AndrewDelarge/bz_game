using game.Gameplay.Characters.Common;
using game.Gameplay.Common;

namespace game.Gameplay.Characters.AI.Behaviour {
	public class BehaviourContext
	{
		public ICharacter target;
		public ICharacter character;
		public BaseStateMachineWithStack<BehaviourState, BaseBehaviourState, BehaviourContext> stateMachine;
	}
}