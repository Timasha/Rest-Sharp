using WebApplication1.src.BuisnessLogic.ExternalServices;
using WebApplication1.src.BuisnessLogic.Models;
using WebApplication1.src.Logger;
using LoggerMessage = WebApplication1.src.Logger.LoggerMessage;

namespace WebApplication1.src.BuisnessLogic
{
    public class BuisnessLogicProvider
    {
        private BaseDatabase _database;
        private Logger.Logger _logger;

        public BuisnessLogicProvider(BaseDatabase database, Logger.Logger logger)
        {
            _database = database;
            _logger = logger;
        }

        public int CreateUser(User user)
        {
            int rowsAffected = 0;
            try
            {
                rowsAffected = _database.CreateUser(user);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return rowsAffected;
        }

        public User GetUser(string login)
        {
            User user = new User();
            try
            {
                user = _database.GetUser(login);
            } catch (Exception ex)
            {
                LoggerMessage loggerMessage = new LoggerMessage($"Databases get user error: {ex.Message}", LogsLevel.Error);
                _logger.Log(loggerMessage);
                Environment.Exit(0);
            }
            return user;
        }
        
        public int UpdateUser(string login, User user)
        {
            int rowsAffected = 0;
            try
            {
                rowsAffected = _database.UpdateUser(login, user);
            }catch (Exception ex)
            {
                LoggerMessage loggerMessage = new LoggerMessage($"Databases update user error: {ex.Message}", LogsLevel.Error);
                _logger.Log(loggerMessage);
                Environment.Exit(0);
            }
            return rowsAffected;
        }

        public int DeleteUser(string login)
        {
            int rowsAffected = 0;
            try
            {
                rowsAffected = _database.DeleteUser(login);
            }catch (Exception ex)
            {
                LoggerMessage loggerMessage = new LoggerMessage($"Databases delete user error: {ex.Message}", LogsLevel.Error);
                _logger.Log(loggerMessage);
                Environment.Exit(0);
            }
            return rowsAffected;
        }
    }
}
