using game.core.InputSystem;

namespace game.Gameplay.Weapon {
	public class WeaponReady : WeaponIdle {
		private const float EXIT_TIME = 2f;

		private float _exitTime;
		public override void Enter() {
			_exitTime = EXIT_TIME;
		}
        
		public override void HandleState(float deltaTime) {
			_exitTime -= deltaTime;

			if (_exitTime <= 0) {
				_context.stateMachine.ReturnState();
			}
		}
	}
}