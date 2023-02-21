﻿using game.Gameplay.Characters;
using game.Gameplay.Characters.Player;
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