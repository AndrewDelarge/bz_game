using game.core.Common;

namespace game.Gameplay.Characters.Player
{
    public abstract class PlayerStateBase<T, TContext> : CharacterState<T, TContext> {
        protected new TContext context => (TContext) base.context;

        public override void Enter()
        {
            AppCore.Get<ILogger>()?.Log($"Enter {GetType()}");
        }

        public override void Exit()
        {
            AppCore.Get<ILogger>()?.Log($"Exit {GetType()}");
        }
    }
}
