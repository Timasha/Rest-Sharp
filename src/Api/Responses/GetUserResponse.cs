using WebApplication1.src.BuisnessLogic.Models;

namespace WebApplication1.src.Api.Responses
{
    public class GetUserResponse:BaseResponse
    {
        public GetUserResponse(string error):base(error) {}

        public User User { get; set; }

        public GetUserResponse(User user) {
            User = user;
        }

    }
}
