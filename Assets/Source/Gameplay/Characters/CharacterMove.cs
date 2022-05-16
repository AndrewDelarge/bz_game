using core.InputSystem.Interfaces;
using game.core.InputSystem;
using game.gameplay.characters;
using UnityEngine;

namespace game.Source.Gameplay.Characters
{
    public struct CharacterMove
    {
        public Vector3 vector { get; private set; }
        public float multiplier { get; private set; }
        public float angle { get; private set; }
        
        public CharacterMove(Vector3 vector, float multiplier, float angle)
        {
            this.vector = vector;
            this.multiplier = multiplier;
            this.angle = angle;
        }

        public void Update(Vector3 vector, float multiplier, float angle)
        {
            this.vector = vector;
            this.multiplier = multiplier;
            this.angle = angle;
        }

        public void Reset()
        {
            vector = default;
            multiplier = default;
            angle = default;
        }
    }
}