using WebApplication1.src.BuisnessLogic.Models;

namespace WebApplication1.src.BuisnessLogic.ExternalServices
{
    public abstract class BaseDatabase
    {

        private protected Logger.Logger _logger;

        public BaseDatabase(Logger.Logger logger)
        {
            _logger = logger;
        }

        public abstract void Connect();

        public abstract void Disconnect();

        public abstract void Migrate();
        public abstract int CreateUser(User user);
        
        public abstract User GetUser(string login);

        public abstract int UpdateUser(string login, User newUser);

        public abstract int DeleteUser(string login);
    }
}
