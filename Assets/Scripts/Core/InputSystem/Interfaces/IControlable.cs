using UnityEngine;

namespace Core.InputSystem.Interfaces
{
    public interface IControlable
    {
        void OnInputKeyPressed(KeyCode keyCode);
        void OnInputKeyDown(KeyCode keyCode);
    }
}