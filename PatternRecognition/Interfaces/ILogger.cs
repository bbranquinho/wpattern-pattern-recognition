namespace WPattern.Pattern.Recognition.Interfaces
{
    public interface ILogger
    {
        void LogMessage(string message);

        void ClearLog();
    }
}