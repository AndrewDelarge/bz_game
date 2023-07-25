using game.core.InputSystem;
using game.Gameplay.Common;

namespace game.Gameplay.Characters.AI.Behaviour {
	public abstract class BaseBehaviourState : IBaseState<BehaviourState, BehaviourContext> {
		protected BehaviourContext _context;
		public abstract BehaviourState type { get; }
		public virtual bool CheckEnterCondition() => true;
		public virtual bool CheckExitCondition() => true;
		public virtual void Enter() { }
		public virtual void Exit() { }
		public virtual void HandleState(float deltaTime) { }
		public virtual void HandleInput(InputData data) { }

		public virtual void Init(BehaviourContext context) {
			_context = context;
		}
	}
}