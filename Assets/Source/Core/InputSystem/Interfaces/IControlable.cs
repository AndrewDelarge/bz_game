using UnityEngine;

namespace Core.InputSystem.Interfaces
{
    public interface IControlable
    {
        void OnVectorInput(Vector3 vector3);
        void OnInputKeyPressed(KeyCode keyCode);
        void OnInputKeyDown(KeyCode keyCode);
    }
}