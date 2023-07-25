namespace game.Gameplay.Characters.Player
{
    public class PlayerActionDeadState : PlayerStateBase<PlayerActionStateEnum, PlayerCharacterContext>
    {
        public override bool CheckExitCondition() => false;
    }
}