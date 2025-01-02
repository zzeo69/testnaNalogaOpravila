using Common.Logger;

namespace Infrastructure.Logger
{
    public class Logger : ILogger
    {
        private readonly string _logFilePath;

        public Logger(string logFilePath)
        {
            _logFilePath = logFilePath;
        }

        public void LogInfo(string message)
        {
            var log = $"[Info] [{DateTime.Now}] {message}";
            File.AppendAllText(_logFilePath, log + Environment.NewLine);
        }

        public void LogError(string message)
        {
            var log = $"[ERROR] [{DateTime.Now}] {message}";
            File.AppendAllText(_logFilePath, log + Environment.NewLine);
        }
    }
}
