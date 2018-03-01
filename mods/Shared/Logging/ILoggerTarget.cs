namespace SharedCode.Logging
{
    public interface ILoggerTarget
    {
        void StartLogging();
        void EndLogging();
        void Log(string text);
    }
}
