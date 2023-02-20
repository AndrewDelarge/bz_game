using game.Source.core.Common;

namespace game.Source.Gameplay.Characters
{

        public abstract class PlayerStateBase<T> : CharacterState<T>
        {
            protected new PlayerCharacterContext context => (PlayerCharacterContext) base.context;

            public override void Enter()
            {
                GCore.Get<ILogger>()?.Log($"Enter {GetType()}");
            }

            public override void Exit()
            {
                GCore.Get<ILogger>()?.Log($"Exit {GetType()}");
            }
        }
    }
