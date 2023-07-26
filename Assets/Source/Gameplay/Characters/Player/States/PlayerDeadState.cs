using game.core.Storage.Data.Character;

namespace game.Gameplay.Characters.Player
{
    public class PlayerDeadState : PlayerStateBase<CharacterStateEnum, PlayerCharacterContext>
    {
        private float _deathLenght;
        public override bool CheckExitCondition() => false;

        public override void Enter() {
            context.animation.PlayAnimation(CharacterAnimationEnum.DEATH);
            context.animation.onAnimationComplete.Add(OnAnimationComplete);
        }

        private void OnAnimationComplete(CharacterAnimationEnum anim) {
            if (anim == CharacterAnimationEnum.DEATH) {
                context.animation.Disable();
            }
        }
    }
}