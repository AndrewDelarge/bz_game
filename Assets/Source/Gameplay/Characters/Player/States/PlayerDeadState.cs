using game.core.Storage.Data.Character;

namespace game.Gameplay.Characters.Player
{
    public class PlayerDeadState : PlayerStateBase<CharacterStateEnum, PlayerCharacterContext>
    {
        private float _deathLenght;
        public override bool CheckExitCondition() => false;

        public override void Enter() {
            context.animation.PlayAnimation(CharacterAnimationEnum.DEATH);
            var data = context.animation.GetAnimationData(CharacterAnimationEnum.DEATH);
        }

        public override void HandleState(float deltaTime)
        {
            if (_deathLenght > 0) {
                _deathLenght -= deltaTime;
                return;
            }
            
            context.animation.Disable();
        }
    }
}