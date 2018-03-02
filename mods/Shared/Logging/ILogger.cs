using System;

namespace SharedCode.Logging
{
    public interface ILogger
    {
        ILogger WithTarget(ILoggerTarget target);

        ILogger Open();

        void LogTrace(string text);
        void LogInfo(string text);
        void LogWarning(string text);
        void LogError(string text);
        void LogFatal(string text);
        void LogException(Exception e, string prependingText);

        void Log(string text);
        void Log(string text, LogLevel logLevel);

        void Close();
    }
}
