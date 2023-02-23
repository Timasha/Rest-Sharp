using MongoDB.Bson.Serialization.Attributes;

namespace WebApplication1.src.BuisnessLogic.Models
{
    public class User
    {
        [BsonId]
        public string Login { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }

        public User()
        {
            Login = "";
            Password = "";
        }

    }
}
