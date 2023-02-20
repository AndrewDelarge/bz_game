using game.core.common;
using game.core.InputSystem;
using game.gameplay.control;
using game.Source.core.Common;
using game.Source.Gameplay.Characters;
using game.Source.Gameplay.Characters.Player;
using UnityEngine;
using ILogger = game.Source.core.Common.ILogger;
using Logger = game.Source.core.Common.Logger;

namespace game.core
{
    public class Client : ClientBase
    {
        protected override void CoreInit()
        {
            GCore.Register<SceneLoader>(new SceneLoader());
            GCore.Register<IInputManager>(new InputManager());
            GCore.Register<ILogger>(new Logger());
        }

        protected override void CoreStart() {
            GCore.Start();
        }

        protected override void GameStart()
        {
            // init input
            var inputListener = Instantiate(Resources.Load<InputListener>("InputListener"));
            
            // init player
            var playerCharacter = Instantiate(Resources.Load<PlayerCharacter>("PlayerView"), Vector3.zero, Quaternion.identity);

            var characterManager = new CharactersManager();
            characterManager.RegisterCharacter(playerCharacter);
            
            GCore.Register<CharactersManager>(characterManager);
            
            #if UNITY_EDITOR 
                return;
            #endif
            GCore.Get<SceneLoader>().LoadScene(1);
        }
    }
}