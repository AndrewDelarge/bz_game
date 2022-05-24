using game.Source.Gameplay.Characters;
using UnityEngine;

namespace game.gameplay.characters
{
    public interface ICharacterMovement
    {
        void Move(CharacterMove move);
        void Rotate(float angle);

        float GetHorizontalVelocity();
        void Disable();
    }
}