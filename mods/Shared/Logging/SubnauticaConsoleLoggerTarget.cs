using System;
using SharedCode.Utils;

namespace SharedCode.Logging
{
    public class SubnauticaConsoleLoggerTarget : ILoggerTarget
    {
        private readonly string _modName;

        public SubnauticaConsoleLoggerTarget(string modName)
        {
            _modName = modName;
        }

        #region Implementation of ILoggerTarget
        public void StartLogging()
        {
        }

        public void EndLogging()
        {
        }

        public void Log(string text)
        {
            Console.WriteLine(
                $"[{DateTimeUtils.GetTimeString(DateTime.Now)}] [{_modName}] {text} {Environment.NewLine}");
        }
        #endregion
    }
}
