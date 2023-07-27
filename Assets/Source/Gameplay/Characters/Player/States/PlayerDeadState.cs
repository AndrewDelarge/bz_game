using game.core.Storage.Data.Character;
using game.Source.core.Common;

namespace game.Gameplay.Characters.Player
{
    public class PlayerDeadState : PlayerStateBase<CharacterStateEnum, PlayerCharacterContext>
    {
        public override bool CheckExitCondition() => false;

        public override void Enter() {
            context.animation.SetMotionVelocityPercent(0, true);
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