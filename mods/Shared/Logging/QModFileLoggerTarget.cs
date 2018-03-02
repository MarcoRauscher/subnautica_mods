using System;
using System.IO;
using SharedCode.Utils;

namespace SharedCode.Logging
{
    public class QModFileLoggerTarget : ILoggerTarget
    {
        private readonly string _fullFilePath;
        private DateTime _startTime;

        public QModFileLoggerTarget(string fullFilePath, LogLevel logLevel)
        {
            _fullFilePath = fullFilePath;
            LogLevel = logLevel;
        }

        public LogLevel LogLevel { get; set; }

        #region Implementation of ILoggerTarget
        public void StartLogging()
        {
            try
            {
                if (File.Exists(_fullFilePath))
                    File.Delete(_fullFilePath);

                _startTime = DateTime.Now;

                File.WriteAllText(_fullFilePath,
                                  $"Begin Logging at {DateTimeUtils.GetTimeString(_startTime)}{Environment.NewLine}");
            }
            catch
            {
                //ignore exceptions for QModFileLoggerTarget
            }
        }

        public void EndLogging()
        {
            try
            {
                File.AppendAllText(_fullFilePath,
                                   $"End Logging at {DateTimeUtils.GetTimeString(DateTime.Now)}{Environment.NewLine}");
            }
            catch
            {
                //ignore exceptions for QModFileLoggerTarget
            }
        }

        public void Log(string text, LogLevel messageLogLevel)
        {
            if (!(messageLogLevel >= LogLevel))
                return;

            try
            {
                File.AppendAllText(_fullFilePath,
                                   $"[{DateTimeUtils.GetTimeString(DateTime.Now)}] {text} {Environment.NewLine}");
            }
            catch
            {
                //ignore exceptions for QModFileLoggerTarget
            }
        }
        #endregion
    }
}
