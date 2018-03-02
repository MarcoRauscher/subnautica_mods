using System;
using SharedCode.Utils;

namespace SharedCode.Logging
{
    public class SubnauticaConsoleLoggerTarget : ILoggerTarget
    {
        private readonly string _modName;

        public SubnauticaConsoleLoggerTarget(string modName, LogLevel logLevel)
        {
            _modName = modName;
            LogLevel = logLevel;
        }

        #region Implementation of ILoggerTarget
        /// <inheritdoc />
        public LogLevel LogLevel { get; set; }

        public void StartLogging()
        {
        }

        public void EndLogging()
        {
        }

        public void Log(string text, LogLevel messageLogLevel)
        {
            Console.WriteLine(
                $"[{DateTimeUtils.GetTimeString(DateTime.Now)}] [{_modName}] {text} {Environment.NewLine}");
        }
        #endregion
    }
}
