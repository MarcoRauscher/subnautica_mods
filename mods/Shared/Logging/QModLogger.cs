using System;
using System.Collections.Generic;
using SharedCode.Utils;

namespace SharedCode.Logging
{
    public class QModLogger : ILogger
    {
        private const LogLevel DefaultLogLevel = LogLevel.Warning;

        private readonly List<ILoggerTarget> _loggerTargets;

        public QModLogger()
        {
            _loggerTargets = new List<ILoggerTarget>();
        }

        public ILogger WithTarget(ILoggerTarget target)
        {
            _loggerTargets.Add(target);
            return this;
        }

        #region Implementation of ILogger
        public void Close()
        {
            foreach (ILoggerTarget target in _loggerTargets)
                target.EndLogging();
        }

        public ILogger Open()
        {
            foreach (ILoggerTarget target in _loggerTargets)
                target.StartLogging();

            return this;
        }

        public void LogTrace(string text)
        {
            Log(text, LogLevel.Trace);
        }

        public void LogInfo(string text)
        {
            Log(text, LogLevel.Info);
        }

        public void LogWarning(string text)
        {
            Log(text, LogLevel.Warning);
        }

        public void LogError(string text)
        {
            Log(text, LogLevel.Error);
        }

        public void LogFatal(string text)
        {
            Log(text, LogLevel.Fatal);
        }

        public void LogException(Exception e, string prependingText)
        {
            string result = prependingText + Environment.NewLine;
            result += ExceptionUtils.GetExceptionErrorString(e);
            LogFatal(result);
        }

        public void Log(string text)
        {
            Log(text, DefaultLogLevel);
        }

        public void Log(string text, LogLevel messageLogLevel)
        {
            foreach (ILoggerTarget target in _loggerTargets)
            {
                if (!(messageLogLevel > target.LogLevel))
                    return;

                target.Log($"[{messageLogLevel.ToString()}] {text}", messageLogLevel);
            }
        }
        #endregion
    }
}
