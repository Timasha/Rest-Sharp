using System;

namespace WebApplication1.src.Logger.ExternalLogger
{
    public class ConsoleLogger : IExternalLogger
    {
        public void Log(LoggerMessage message)
        {
            switch(message.LogLevel){
                case (LogsLevel.Info):
                {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("(info)   ");
                        break;
                }
                case (LogsLevel.Debug): 
                {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("(debug)  ");
                        break;
                }
                case (LogsLevel.Trace):
                {
                        Console.Write("(trace)  ");
                        break;
                }
                case (LogsLevel.Warning): 
                {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("(warning)");
                        break;
                }
                case (LogsLevel.Error):
                {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("(error)  ");
                        break;
                }
                case(LogsLevel.Fatal):
                {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write("(fatal)  ");

                        break;
                }

               
            }
            //Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" [" + message.Time.ToString() + "] : " + message.Message);
        }
    }
}
