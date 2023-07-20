using System;
using game.core.InputSystem;

namespace game.Gameplay.Common {
	public interface IBaseState<TType, TContext> : IBaseState
	{
		TType type { get; }
		void Init(TContext context);
	}
	public interface IBaseState
	{
		virtual bool CheckEnterCondition() => true;
		virtual bool CheckExitCondition() => true;

		void Enter();

		void Exit();

		void HandleState(float deltaTime);
        
		void HandleInput(InputData data);
	}
}