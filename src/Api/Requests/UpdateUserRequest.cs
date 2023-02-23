using WebApplication1.src.BuisnessLogic.Models;

namespace WebApplication1.src.Api.Requests
{
    public class UpdateUserRequest
    {
        public string Login { get; set; }
        public User User { get; set; }
    }
}
