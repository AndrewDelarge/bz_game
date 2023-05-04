
namespace game.Gameplay.Characters.Common
{
    public interface ICharacterMovement
    {
        void Move(CharacterMove move);
        void Rotate(float angle);

        float GetHorizontalVelocity();
        void Disable();
        void SetLockRotation(bool value);
    }
}