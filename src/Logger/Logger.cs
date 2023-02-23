using WebApplication1.src.Logger.ExternalLogger;
using WebApplication1.src.Logger.ExternalLogger.factory;

namespace WebApplication1.src.Logger
{
    public class Logger
    {
        private List<IExternalLogger> _loggers = new List<IExternalLogger>();
        private bool _debug;
        public void Log(LoggerMessage loggerMessage)
        {
            if (!_debug && loggerMessage.LogLevel == LogsLevel.Debug)
            {
                return;
            }
            foreach(var logger in _loggers)
            {
                logger.Log(loggerMessage);
            }
        }

        public Logger(bool debug, IExternalLogger logger, params IExternalLogger[] loggers)
        {
            _loggers.Add(logger);
            _loggers.Concat(loggers);
            _debug = debug;
        }

        public void AddLogger(IExternalLogger logger)
        {
            _loggers.Append(logger);
        }

        public void RemoveLogger(IExternalLogger logger)
        {
            _loggers.Remove(logger);
        }
    }
}
