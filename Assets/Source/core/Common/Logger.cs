using game.core.common;
using UnityEngine;

namespace game.Source.core.Common
{
    public class Logger : ILogger, ICoreManager
    {
        public Logger() {}


        public void Log(string text)
        {
            Debug.Log(text);
        }
    }

    public interface ILogger
    {
        void Log(string text);
    }
}