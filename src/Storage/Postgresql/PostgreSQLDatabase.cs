using Npgsql;
using WebApplication1.src.BuisnessLogic.ExternalServices;
using WebApplication1.src.BuisnessLogic.Models;
using WebApplication1.src.Logger;
using LoggerMessage = WebApplication1.src.Logger.LoggerMessage;

namespace WebApplication1.src.Storage.Postgresql
{
    public class PostgreSQLDatabase:BaseDatabase
    {

        private PostgreSQLConfig _config;
        private NpgsqlConnection _conn;

        public PostgreSQLDatabase(PostgreSQLConfig config, Logger.Logger logger):base(logger)
        {
            _config = config;
        }

        public override void Connect()
        {
            var connString = "Host="+_config.Ip+";Username="+_config.Login+";Password="+_config.Password+";Database=users";
            
            try
            {
                _conn = new NpgsqlConnection(connString);
                _conn.Open();
            }catch(Exception ex)
            {
                LoggerMessage loggerMessage = new LoggerMessage($"Postgresql connection error: {ex.Message}", LogsLevel.Fatal);
                _logger.Log(loggerMessage);
                Environment.Exit(0);
            }
            var connectedMessage = new LoggerMessage("Postgresql connected", LogsLevel.Info);
            _logger.Log(connectedMessage);
        }

        public override void Disconnect()
        {
            _conn.Close();
        }

        public override void Migrate()
        {
            try
            {
                var command = new NpgsqlCommand("CREATE TABLE IF NOT EXISTS users" +
                    "(" +
                    "Login TEXT PRIMARY KEY," +
                    "Password TEXT" +
                    ");", _conn);
                command.ExecuteNonQuery();
            }catch (Exception ex)
            {
                LoggerMessage loggerMessage = new LoggerMessage($"Migrate database error: {ex.Message}", LogsLevel.Fatal);
                _logger.Log(loggerMessage);
                Environment.Exit(0);
            }
        }

        public override int CreateUser(User user)
        {
            int rowsAffected = 0;
            try
            {
                var command = new NpgsqlCommand($"INSERT INTO users VALUES (@Login,@Password);", _conn);
                command.Parameters.AddWithValue("Login", user.Login);
                command.Parameters.AddWithValue("Password", user.Password);
                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                PostgresException pex = ex as PostgresException;
                if (pex !=null && int.Parse(pex.SqlState) == 23505)
                {
                    return 0;
                }
                throw ex;
            }
            return rowsAffected;
        }

        public override User GetUser(string login)
        {
            try
            {
                var command = new NpgsqlCommand($"SELECT * FROM users WHERE Login = @Login;", _conn);
                command.Parameters.AddWithValue("Login", login);
                var reader = command.ExecuteReader();
                User user;
                if (!reader.Read())
                {
                    reader.Close();
                    return null;
                }
                user = new User { Login = reader["Login"].ToString(), Password = reader["Password"].ToString() };
                reader.Close();
                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        

        public override int UpdateUser(string login, User newUser)
        {
            int rowsAffected = 0;
            try
            {
                var command = new NpgsqlCommand($"UPDATE users SET Login = @NewLogin, Password = @NewPassword WHERE Login = @Login;", _conn);
                command.Parameters.AddWithValue("NewLogin", newUser.Login);
                command.Parameters.AddWithValue("NewPassword", newUser.Password);
                command.Parameters.AddWithValue("Login", login);
                rowsAffected = command.ExecuteNonQuery();
            }catch (Exception ex)
            {
                throw ex;
            }
            return rowsAffected;
        }

        public override int DeleteUser(string login)
        {
            int rowsAffected = 0;
            try
            {
                var command = new NpgsqlCommand($"DELETE FROM users WHERE Login = @Login;", _conn);
                command.Parameters.AddWithValue("@Login", login);
                rowsAffected = command.ExecuteNonQuery();
            }catch(Exception ex)
            {
                throw ex;
            }
            return rowsAffected;
        }
    }
}
