namespace SharedCode.Logging
{
    public interface ILoggerTarget
    {
        LogLevel LogLevel { get; set; }

        void StartLogging();
        void EndLogging();
        void Log(string text, LogLevel messageLogLevel);
    }
}
