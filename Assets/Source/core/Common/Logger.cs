using game.core.common;
using UnityEngine;

namespace game.core.Common
{
    public class Logger : ILogger, ICoreManager
    {
        public Logger() {}


        public void Log(string text) {
            Debug.Log(text);
        }

        public void Error(string text) {
            Debug.LogError(text);
        }
    }

    public interface ILogger
    {
        void Log(string text);
        void Error(string text);
    }
}