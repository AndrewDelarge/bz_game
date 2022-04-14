using game.core.InputSystem;

namespace game.Source.Gameplay.Characters
{
    public partial class Character
    {
        public class PlayerActionIdleState : PlayerStateBase
        {
            public PlayerActionIdleState(Character character) : base(character)
            {
            }

            public override void HandleState()
            {
            }

            public override void HandleInput(InputData data)
            {
            }
        }
    }
}