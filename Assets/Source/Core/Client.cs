using game.core.common;
using game.core.InputSystem;

namespace game.core
{
    public class Client : ClientBase
    {
        protected override void CoreInit()
        {
            Core.Register<InputManager>(new InputManager());
        }
    }
}