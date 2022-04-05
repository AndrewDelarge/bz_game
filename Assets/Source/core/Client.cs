using game.core.common;
using game.core.InputSystem;
using game.gameplay.control;
using game.Source.Gameplay.Characters;
using UnityEngine;

namespace game.core
{
    public class Client : ClientBase
    {
        protected override void CoreInit()
        {
            Core.Register<SceneLoader>(new SceneLoader());
            Core.Register<IInputManager>(new InputManager());
        }

        protected override void CoreStart() {
            Core.Start();
        }

        protected override void GameStart()
        {
            // init input
            var inputListener = Instantiate(Resources.Load<InputListener>("InputListener"));
            
            // init player
            var playerCharacter = Instantiate(Resources.Load<Character>("PlayerView"));
            playerCharacter.Init();
            
            
            #if UNITY_EDITOR 
                return;
            #endif
            Core.Get<SceneLoader>().LoadScene(1);
        }
    }
}