namespace WebApplication1.src.Logger.ExternalLogger.factory
{
    public class ConsoleLoggerFactory:BaseExternalLoggerFactory
    {
        public override IExternalLogger Create()
        {
            return new ConsoleLogger();
        }
    }
}
