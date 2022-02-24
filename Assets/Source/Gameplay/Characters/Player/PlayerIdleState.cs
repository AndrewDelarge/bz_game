using UnityEngine;

namespace game.Source.Gameplay.Characters
{
    public partial class Character
    {
        public class PlayerIdleState : PlayerStateBase
        {
            public PlayerIdleState(Character character) : base(character) {}

            public override void HandleState()
            {
                if (character._moves.Count != 0)
                {
                    character._stateMachine.ChangeState(character._moveState);
                }
            }
        }
    }
}