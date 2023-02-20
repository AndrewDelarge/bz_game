using game.core.InputSystem;

namespace game.Source.Gameplay.Characters.Common {
	public abstract class BaseState
	{
		public virtual bool CheckEnterCondition() => true;

		public abstract void Enter();

		public abstract void Exit();

		public abstract void HandleState();
        
		public abstract void HandleInput(InputData data);
	}
}