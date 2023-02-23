using System.Text.Json;


namespace WebApplication1.src.Storage.Mongo
{
    public class MongoDatabaseConfig
    {
        public string Ip { get; set; } = ""; 
        public string Port { get; set; } = ""; 
        public string DbName { get; set; } = "";
        public string MongoLogin { get; set; } = "";
        public string MongoPassword { get; set; } = "";

        public MongoDatabaseConfig()
        {

        }
    }
}
