using game.core.common;
using game.core.InputSystem;
using game.core.level;
using game.Gameplay.Characters.Common;
using game.Source.core.Common;
using UnityEngine;
using ILogger = game.core.Common.ILogger;
using Logger = game.core.Common.Logger;

namespace game.core
{
    public class Client : ClientBase {
        private GameTimer _timer;
        private LevelManager _levelManager;

        protected override void CoreInit()
        {
            AppCore.Register<CameraManager>(new CameraManager());
            AppCore.Register<LevelManager>(new LevelManager());
            AppCore.Register<IInputManager>(new InputManager());
            AppCore.Register<ILogger>(new Logger());
            AppCore.Register<GameTimer>(new GameTimer());
        }

        protected override void CoreStart() {
            AppCore.Start();
            
            _timer = AppCore.Get<GameTimer>();
            _levelManager = AppCore.Get<LevelManager>();
        }

        protected override void GameStart()
        {
            #if UNITY_EDITOR
                return;
            #endif
            AppCore.Get<LevelManager>().Load(1);
        }

        protected override void Dispose() {
            AppCore.Dispose();
        }

        private void Update() {
            _timer.Update(Time.deltaTime);
            _levelManager.Update(Time.deltaTime);
        }
    }
}