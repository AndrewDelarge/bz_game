using game.core.InputSystem;
using UnityEngine;

namespace core.InputSystem.Interfaces
{
    public interface IControlable 
    {
        bool isListen { get; }
        void OnDataUpdate(InputData data);
    }
}