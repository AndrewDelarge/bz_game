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
		bool CheckEnterCondition();
		bool CheckExitCondition();

		void Enter();

		void Exit();

		void HandleState(float deltaTime);
        
		void HandleInput(InputData data);
	}
}