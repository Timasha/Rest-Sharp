namespace WebApplication1.src.Api.Responses
{
    public abstract class BaseResponse
    {
        public string Error { get; set; } = "";
        public BaseResponse(string error)
        {
            Error = error;
        }
        public BaseResponse() { }
    }
}
