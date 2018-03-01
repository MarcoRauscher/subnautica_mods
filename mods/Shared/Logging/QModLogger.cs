using System;
using System.Collections.Generic;

namespace SharedCode.Logging
{
    public class QModLogger : ILogger
    {
        private const LogLevel DefaultLogLevel = LogLevel.Warning;

        private readonly List<ILoggerTarget> _loggerTargets;
        private readonly LogLevel _logLevel;
        private readonly string _defaultModule;

        public QModLogger(LogLevel logLevel, string defaultModule)
        {
            _logLevel = logLevel;
            _loggerTargets = new List<ILoggerTarget>();
            _defaultModule = defaultModule;
        }

        private bool ShouldLog(LogLevel messageLogLevel)
        {
            return messageLogLevel >= _logLevel;
        }

        #region Implementation of ILogger
        public void Close()
        {
            foreach (ILoggerTarget target in _loggerTargets)
                target.EndLogging();
        }

        public void Open()
        {
            foreach (ILoggerTarget target in _loggerTargets)
                target.StartLogging();
        }

        public void AddLoggerTarget(ILoggerTarget target)
        {
            _loggerTargets.Add(target);
        }

        public void LogTrace(string text, string module)
        {
            Log(text, module, LogLevel.Trace);
        }

        public void LogInfo(string text, string module)
        {
            Log(text, module, LogLevel.Info);
        }

        public void LogWarning(string text, string module)
        {
            Log(text, module, LogLevel.Warning);
        }

        public void LogError(string text, string module)
        {
            Log(text, module, LogLevel.Error);
        }

        public void LogFatal(string text, string module)
        {
            Log(text, module, LogLevel.Fatal);
        }

        public void LogException(Exception e, string module)
        {
            string text = e.Message + Environment.NewLine + e.StackTrace;
            LogFatal(text, module);
        }

        public void LogTrace(string text)
        {
            LogTrace(text, _defaultModule);
        }

        public void LogInfo(string text)
        {
            LogInfo(text, _defaultModule);
        }

        public void LogWarning(string text)
        {
            LogWarning(text, _defaultModule);
        }

        public void LogError(string text)
        {
            LogError(text, _defaultModule);
        }

        public void LogFatal(string text)
        {
            LogFatal(text, _defaultModule);
        }

        public void LogException(Exception e)
        {
            LogException(e, _defaultModule);
        }

        public void Log(string text)
        {
            Log(text, _defaultModule);
        }

        public void Log(string text, string module)
        {
            Log(text, module, DefaultLogLevel);
        }

        public void Log(string text, string module, LogLevel logLevel)
        {
            if (!ShouldLog(logLevel))
                return;

            foreach (ILoggerTarget target in _loggerTargets)
            {
                target.Log($"[{logLevel.ToString()}] [{module}] {text}");
            }
        }
        #endregion
    }
}
