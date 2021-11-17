using game.core.common;
using game.core.InputSystem;

namespace game.core
{
    public class Client : ClientBase
    {
        protected override void CoreInit()
        {
            Core.Register<SceneLoader>(new SceneLoader());
            Core.Register<InputManager>(new InputManager());
        }

        protected override void CoreStart() {
            Core.Start();
        }

        protected override void GameStart() {
            // Core.Get<SceneLoader>().LoadScene(1);
        }
    }
}