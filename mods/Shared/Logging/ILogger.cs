using System;

namespace SharedCode.Logging
{
    public interface ILogger
    {
        void AddLoggerTarget(ILoggerTarget target);

        void Open();

        void LogTrace(string text);
        void LogInfo(string text);
        void LogWarning(string text);
        void LogError(string text);
        void LogFatal(string text);
        void LogException(Exception e);

        void LogTrace(string text, string module);
        void LogInfo(string text, string module);
        void LogWarning(string text, string module);
        void LogError(string text, string module);
        void LogFatal(string text, string module);
        void LogException(Exception e, string module);

        void Log(string text);
        void Log(string text, string module);
        void Log(string text, string module, LogLevel logLevel);

        void Close();
    }
}
