using game.core.InputSystem;
using UnityEngine;

namespace game.core.InputSystem.Interfaces
{
    public interface IControlable 
    {
        bool isListen { get; }
        void OnDataUpdate(InputData data);
    }
}