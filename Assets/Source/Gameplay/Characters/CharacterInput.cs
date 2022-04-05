using core.InputSystem.Interfaces;
using game.core.InputSystem;
using game.gameplay.characters;
using UnityEngine;

namespace game.Source.Gameplay.Characters
{
    public class CharacterInput : IControlable
    {
        public bool isListen { get; }
        private CharacterMove _move { get; } = new CharacterMove(Vector3.zero, 0f, 0f);
        public CharacterMove move => _move;

        private void Init()
        {
            
        }

        public void OnVectorInput(Vector3 vector3)
        {
            _move.Update(vector3, 0, 0);
        }

        public void OnInputKeyPressed(KeyCode keyCode)
        {
            throw new System.NotImplementedException();
        }

        public void OnInputKeyDown(KeyCode keyCode)
        {
            throw new System.NotImplementedException();
        }

        public void OnInputKeyUp(KeyCode keyCode)
        {
            throw new System.NotImplementedException();
        }

        public void OnDataUpdate(InputData data)
        {
            throw new System.NotImplementedException();
        }
    }
    
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