namespace WebApplication1.src.Logger
{
    public class LoggerMessage
    {
        private DateTime _time;
        private string _message;
        private LogsLevel _logLevel;

        public DateTime Time { get { return _time; } }
        public string Message { get { return _message; } }

        public LogsLevel LogLevel { get { return _logLevel; } }

        public LoggerMessage(string message, LogsLevel logLevel)
        {
            _time = DateTime.Now;
            _message = message;
            _logLevel = logLevel;
        }
    }
}
