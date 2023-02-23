using WebApplication1.src.Storage.Mongo;
using WebApplication1.src.Storage.Postgresql;

namespace WebApplication1.src.config
{
    public class ServerConfig
    {
        public string Ip { get; set; }
        public string Port { get; set; }

        public bool Debug { get; set; }

        public string DatabaseType { get; set; }

        public PostgreSQLConfig PostgreSQLConfig { get; set; }

        public MongoDatabaseConfig MongoDatabaseConfig { get; set; }

        public ServerConfig() { }
    }
}
