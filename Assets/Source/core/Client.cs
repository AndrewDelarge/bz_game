using game.core.common;
using game.core.InputSystem;
using game.gameplay.control;
using game.Gameplay.Characters.Player;
using game.Source.Gameplay.Weapon;
using UnityEngine;
using ILogger = game.core.Common.ILogger;
using Logger = game.core.Common.Logger;

namespace game.core
{
    public class Client : ClientBase
    {
        protected override void CoreInit()
        {
            AppCore.Register<SceneLoader>(new SceneLoader());
            AppCore.Register<IInputManager>(new InputManager());
            AppCore.Register<ILogger>(new Logger());
        }

        protected override void CoreStart() {
            AppCore.Start();
        }

        protected override void GameStart()
        {
            // init input
            var inputListener = Instantiate(Resources.Load<InputListener>("InputListener"));
            
            // init player
            var playerCharacter = Instantiate(Resources.Load<PlayerCharacter>("PlayerView"), Vector3.zero, Quaternion.identity);

            var characterManager = new CharactersManager();
            characterManager.RegisterCharacter(playerCharacter);
            
            AppCore.Register<CharactersManager>(characterManager);
            #if UNITY_EDITOR 
                return;
            #endif
            AppCore.Get<SceneLoader>().LoadScene(1);
        }

        private void Start()
        {
            // TODO костыль пока что
            AppCore.Get<LevelManager>().Add(new ProjectileManager());
        }
    }
}