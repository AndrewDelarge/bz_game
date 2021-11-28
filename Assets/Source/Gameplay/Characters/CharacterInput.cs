using Core.InputSystem.Interfaces;
using game.core.InputSystem;
using UnityEngine;

namespace game.Source.Gameplay.Characters
{
    public class CharacterInput : MonoBehaviour, IControlable
    {
        private Character _character;
        
        private void Awake()
        {
            game.Core.Get<InputManager>().RegisterControlable(this);
        }

        public void OnVectorInput(Vector3 vector3)
        {
            throw new System.NotImplementedException();
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
    }
}