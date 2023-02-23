using Npgsql;
using System.Text.Json;
using WebApplication1.src.Api;
using WebApplication1.src.BuisnessLogic;
using WebApplication1.src.BuisnessLogic.ExternalServices;
using WebApplication1.src.config;
using WebApplication1.src.Logger;
using WebApplication1.src.Logger.ExternalLogger.factory;
using WebApplication1.src.Storage.Postgresql;
using LoggerMessage = WebApplication1.src.Logger.LoggerMessage;

namespace WebApplication1
{
    class Program
    {
        public static void Main(string[] args)
        {
            BaseExternalLoggerFactory consoleLoggerFactory = new ConsoleLoggerFactory();
            Logger logger = new Logger(true,consoleLoggerFactory.Create());



            string? configPath = Environment.GetEnvironmentVariable("CONFIG_PATH");
            if (configPath == null || configPath == "")
            {
                LoggerMessage configErrMessage = new LoggerMessage("Open config error: no path enviroment variable", LogsLevel.Fatal);
                logger.Log(configErrMessage);
                return;
            }



            ServerConfig config;
            try
            {
                var file = File.Open(configPath, FileMode.Open);
                config = JsonSerializer.Deserialize<ServerConfig>(file);
                file.Close();
            }catch (Exception ex)
            {
                LoggerMessage configErrMessage = new LoggerMessage($"Open config error: {ex.Message}", LogsLevel.Fatal);
                logger.Log(configErrMessage);
                return;
            }

            BaseDatabase database = new PostgreSQLDatabase(config.PostgreSQLConfig, logger);

            try
            {
                database.Connect();
                database.Migrate();
            }
            catch (Exception ex)
            {
            }

            BuisnessLogicProvider logicProvider = new BuisnessLogicProvider(database, logger);

            ApiProvider apiProvider = new ApiProvider(logger, logicProvider);

            var builder = WebApplication.CreateBuilder();
            var app = builder.Build();

            app.MapPost("/create", apiProvider.CreateUser);
            app.MapGet("/get", apiProvider.GetUser);
            app.MapPut("/update",apiProvider.UpdateUser);
            app.MapDelete("/delete",apiProvider.DeleteUser);

            app.Run();

            database.Disconnect();
        }
    }
}
