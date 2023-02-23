using MongoDB.Driver;
using WebApplication1.src.Logger;
using LoggerMessage = WebApplication1.src.Logger.LoggerMessage;
using MongoDB.Bson;
using System.Diagnostics.Eventing.Reader;
using MongoDB.Bson.Serialization.Attributes;
using WebApplication1.src.BuisnessLogic.Models;
using WebApplication1.src.BuisnessLogic.ExternalServices;

namespace WebApplication1.src.Storage.Mongo
{
    // not working
    public class MongoDatabase : BaseDatabase
    { 
        private MongoDatabaseConfig _config;

        private MongoClient _mongoClient;
        private IMongoDatabase _database;
        private IMongoCollection<User> _userCollection;

        public MongoDatabaseConfig Config
        {
            get 
            {
                return _config;
            }
        }
        public MongoDatabase(MongoDatabaseConfig config, Logger.Logger logger):base(logger)
        {
            _config = config;
        }

        public override void Connect()
        {
            string loginData = "";
            if (_config.MongoLogin != "" && _config.MongoPassword != "")
            {
                loginData = _config.MongoLogin + ":" + _config.MongoPassword + "@";
            }

            try
            {
                _mongoClient = new MongoClient("mongodb://"+loginData+_config.Ip+":"+_config.Port);
            }
            catch (Exception e)
            {
                LoggerMessage loggerMessage = new LoggerMessage($"MongoDb connection error: {e.Message}",LogsLevel.Fatal);
                _logger.Log(loggerMessage);
                Environment.Exit(0);
            }

            try
            {
                _database = _mongoClient.GetDatabase(_config.DbName);
            }
            catch(Exception e)
            {
                LoggerMessage loggerMessage = new LoggerMessage($"MongoDb database connection error: {e.Message}", LogsLevel.Fatal);
                _logger.Log(loggerMessage);
                Environment.Exit(0);
            }

            try
            {
                _userCollection = _database.GetCollection<User>("users");
            }
            catch(Exception e)
            {
                LoggerMessage loggerMessage = new LoggerMessage($"MongoDb users collection connection error: {e.Message}", LogsLevel.Fatal);
                _logger.Log(loggerMessage);
                Environment.Exit(0);
            }
            LoggerMessage connectionMessage = new LoggerMessage("MongoDb connection complete", LogsLevel.Info);
            _logger.Log(connectionMessage);
        }

        public override void Disconnect()
        {
            throw new NotImplementedException();
        }

        public override void Migrate()
        {
            throw new NotImplementedException();
        }
        public override int CreateUser(User user)
        {
            try
            {
                _userCollection.InsertOne(user);
            }
            catch(Exception e)
            {
                LoggerMessage loggerMessage = new LoggerMessage($"Create user error: {e.Message}", LogsLevel.Error);
                _logger.Log(loggerMessage);
                throw e;
            }
            return 0;
        }

        public override User GetUser(string login)
        {
            User user = new User();
            try
            {
                var filter = Builders<User>.Filter.Eq((u) => u.Login, login);
                user = _userCollection.Find(filter).First();
            }
            catch(Exception e)
            {
                LoggerMessage loggerMessage = new LoggerMessage($"Get user error: {e.Message}", LogsLevel.Error);
                _logger.Log(loggerMessage);
                throw e;
            }
            return user;
        }

        public override int UpdateUser(string login, User newUser)
        {
            try
            {
                var filter = Builders<User>.Filter.Eq((u) => u.Login, login);
                _userCollection.ReplaceOne(filter,newUser);
            }
            catch (Exception e)
            {
                LoggerMessage loggerMessage = new LoggerMessage($"Update user error: {e.Message}", LogsLevel.Error);
                _logger.Log(loggerMessage);
                throw e;
            }
            return 0;
        }
        public override int DeleteUser(string login)
        {
            try
            {
                var filter = Builders<User>.Filter.Eq((u) => u.Login, login);
                _userCollection.DeleteOne(filter);
            }
            catch(Exception e)
            {
                LoggerMessage loggerMessage = new LoggerMessage($"Delete user error: {e.Message}", LogsLevel.Error);
                _logger.Log(loggerMessage);
                throw e;
            }
            return 0;
        }
    }
}
