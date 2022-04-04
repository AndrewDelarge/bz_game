using UnityEngine;

namespace core.InputSystem.Interfaces
{
    public interface IControlable 
    {
        bool isListen { get; }
        void OnVectorInput(Vector3 vector3);
        void OnInputKeyPressed(KeyCode keyCode);
        void OnInputKeyDown(KeyCode keyCode);
        void OnInputKeyUp(KeyCode keyCode);
    }
}