using game.core.common;
using game.core.InputSystem;
using game.gameplay.control;
using game.Source.Gameplay.Characters;
using game.Source.Gameplay.Characters.Player;
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
            var playerCharacter = Instantiate(Resources.Load<PlayerCharacter>("PlayerView"), Vector3.zero, Quaternion.identity);

            var characterManager = new CharactersManager();
            characterManager.RegisterCharacter(playerCharacter);
            
            Core.Register<CharactersManager>(characterManager);
            
            #if UNITY_EDITOR 
                return;
            #endif
            Core.Get<SceneLoader>().LoadScene(1);
        }
    }
}