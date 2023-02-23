using System.Text;
using System.Text.Json;
using WebApplication1.src.Api.Requests;
using WebApplication1.src.Api.Responses;
using WebApplication1.src.BuisnessLogic;
using WebApplication1.src.BuisnessLogic.Models;
using WebApplication1.src.Logger;
using LoggerMessage = WebApplication1.src.Logger.LoggerMessage;

namespace WebApplication1.src.Api
{
    public class ApiProvider
    {
        private Logger.Logger _logger;
        private BuisnessLogicProvider _logicProvider;

        public ApiProvider(Logger.Logger logger, BuisnessLogicProvider logicProvider)
        {
            _logger = logger;
            _logicProvider = logicProvider;
        }

        public Task CreateUser(HttpContext ctx)
        {
            CreateUserRequest? req;
            /*string body = "";
            using (StreamReader reqReader = new StreamReader(ctx.Request.Body))
            {
                Task<string> readerTask = reqReader.ReadToEndAsync();
                readerTask.Wait();
                body = readerTask.Result;
            }*/
            try
            {
                var deserializeTask = JsonSerializer.DeserializeAsync<CreateUserRequest>(ctx.Request.Body);
                req = deserializeTask.Result;

            }catch(Exception ex)
            {
                _logger.Log(new LoggerMessage($"JSON deserialize exception: {ex.Message}", LogsLevel.Error));
                ctx.Response.StatusCode = 400;

                CreateUserResponse resp = new CreateUserResponse($"JSON deserialize exception: {ex.Message}");

                string strResp = JsonSerializer.Serialize(resp);

                ctx.Response.WriteAsync(strResp);

                return Task.CompletedTask;
            }

            if (req == null)
            {
                ctx.Response.StatusCode = 400; 
                CreateUserResponse resp = new CreateUserResponse("Cannot parse request body");

                string strResp = JsonSerializer.Serialize(resp);

                ctx.Response.WriteAsync(strResp);
                return Task.CompletedTask;
            }

            int rowsAffected = 0;


            try
            {
                rowsAffected = _logicProvider.CreateUser(req.User);
            }catch (Exception ex)
            {
                
                _logger.Log(new LoggerMessage($"Create user error: {ex.Message}", LogsLevel.Error));
                
                ctx.Response.StatusCode = 500;

                CreateUserResponse resp = new CreateUserResponse("Internal server error");

                string strResp = JsonSerializer.Serialize(resp);

                ctx.Response.WriteAsync(strResp);
                return Task.CompletedTask;
            }

            if (rowsAffected == 0)
            {
                ctx.Response.StatusCode = 400;
                CreateUserResponse resp = new CreateUserResponse("User already exists");

                string strResp = JsonSerializer.Serialize(resp);

                ctx.Response.WriteAsync(strResp);
                return Task.CompletedTask;
            }
            
            CreateUserResponse successResp = new CreateUserResponse();

            string successStrResp = JsonSerializer.Serialize(successResp);

            ctx.Response.WriteAsync(successStrResp);

            return Task.CompletedTask;
        }

        public Task GetUser(HttpContext ctx)
        {
            GetUserRequest? req;
            try
            {
                var deserializeTask = JsonSerializer.DeserializeAsync<GetUserRequest>(ctx.Request.Body);
                req = deserializeTask.Result;

            }
            catch (Exception ex)
            {
                _logger.Log(new LoggerMessage($"JSON deserialize exception: {ex.Message}", LogsLevel.Error));
                ctx.Response.StatusCode = 400;
                
                GetUserResponse errResp = new GetUserResponse($"JSON deserialize exception: {ex.Message}");

                string strErrResp = JsonSerializer.Serialize(errResp);

                ctx.Response.WriteAsync(strErrResp);

                return Task.CompletedTask;
            }

            if (req == null)
            {
                ctx.Response.StatusCode = 400;

                GetUserResponse errResp = new GetUserResponse($"Cannot parse request body");

                string strErrResp = JsonSerializer.Serialize(errResp);

                ctx.Response.WriteAsync(strErrResp);

                return Task.CompletedTask;
            }

            User gettedUser;

            try
            {
                gettedUser = _logicProvider.GetUser(req.Login);
            }catch (Exception ex)
            {
                _logger.Log(new LoggerMessage($"Get user error: {ex.Message}", LogsLevel.Error));
                ctx.Response.StatusCode = 500;

                GetUserResponse errResp = new GetUserResponse("Internal server error");

                string strErrResp = JsonSerializer.Serialize(errResp);

                ctx.Response.WriteAsync(strErrResp);

                return Task.CompletedTask;
            }

            if (gettedUser == null)
            {

                GetUserResponse errResp = new GetUserResponse("User not found");

                string strErrResp = JsonSerializer.Serialize(errResp);

                ctx.Response.WriteAsync(strErrResp);


                return Task.CompletedTask;
            }


            GetUserResponse resp = new GetUserResponse(gettedUser);

            string userJson = JsonSerializer.Serialize(gettedUser);

            ctx.Response.StatusCode = 200;

            Task task = ctx.Response.WriteAsync(userJson);

            task.Wait();

            return Task.CompletedTask;
        }

        public Task UpdateUser(HttpContext ctx)
        {
            UpdateUserRequest? req;
            try
            {
                var deserializeTask = JsonSerializer.DeserializeAsync<UpdateUserRequest>(ctx.Request.Body);
                req = deserializeTask.Result;
            }
            catch(Exception ex)
            {
                _logger.Log(new LoggerMessage($"JSON deserialize exception: {ex.Message}", LogsLevel.Error));
                ctx.Response.StatusCode = 400;

                GetUserResponse errResp = new GetUserResponse($"JSON deserialize exception: {ex.Message}");

                string strErrResp = JsonSerializer.Serialize(errResp);

                ctx.Response.WriteAsync(strErrResp);


                return Task.CompletedTask;
            }

            if (req == null)
            {

                ctx.Response.StatusCode = 400;

                GetUserResponse errResp = new GetUserResponse($"Cannot parse request body");

                string strErrResp = JsonSerializer.Serialize(errResp);

                ctx.Response.WriteAsync(strErrResp);

                return Task.CompletedTask;
            }

            int rowsAffected = 0;
            try
            {
                rowsAffected = _logicProvider.UpdateUser(req.Login, req.User);
            }
            catch (Exception ex)
            {
                _logger.Log(new LoggerMessage($"Update user error: {ex.Message}", LogsLevel.Error));
                ctx.Response.StatusCode = 500;

                GetUserResponse errResp = new GetUserResponse("Internal server error");

                string strErrResp = JsonSerializer.Serialize(errResp);

                ctx.Response.WriteAsync(strErrResp);

                return Task.CompletedTask;
            }

            if (rowsAffected == 0)
            {
                ctx.Response.StatusCode = 400;
                UpdateUserResponse errResp = new UpdateUserResponse("User does not exists");

                string strErrResp = JsonSerializer.Serialize(errResp);

                ctx.Response.WriteAsync(strErrResp);

                return Task.CompletedTask;
            }

            UpdateUserResponse resp = new UpdateUserResponse();

            string userJson = JsonSerializer.Serialize(resp);

            ctx.Response.StatusCode = 200;

            Task task = ctx.Response.WriteAsync(userJson);

            task.Wait();


            return Task.CompletedTask;
        }


        public Task DeleteUser(HttpContext ctx)
        {
            DeleteUserRequest? req;

            try
            {
                var deserializeTask = JsonSerializer.DeserializeAsync<DeleteUserRequest>(ctx.Request.Body);
                req = deserializeTask.Result;
            }
            catch (Exception ex)
            {
                _logger.Log(new LoggerMessage($"JSON deserialize exception: {ex.Message}", LogsLevel.Error));
                ctx.Response.StatusCode = 400;

                DeleteUserResponse errResp = new DeleteUserResponse($"JSON deserialize exception: {ex.Message}");

                string strErrResp = JsonSerializer.Serialize(errResp);

                ctx.Response.WriteAsync(strErrResp);


                return Task.CompletedTask;
            }

            if (req == null)
            {
                ctx.Response.StatusCode = 400;

                DeleteUserResponse errResp = new DeleteUserResponse($"Cannot parse request body");

                string strErrResp = JsonSerializer.Serialize(errResp);

                ctx.Response.WriteAsync(strErrResp);

                return Task.CompletedTask;
            }

            int rowsAffected = 0;
            try
            {
                rowsAffected = _logicProvider.DeleteUser(req.Login);
            }
            catch (Exception ex)
            {

                _logger.Log(new LoggerMessage($"Delete user error: {ex.Message}", LogsLevel.Error));
                ctx.Response.StatusCode = 500;

                DeleteUserResponse errResp = new DeleteUserResponse("Internal server error");

                string strErrResp = JsonSerializer.Serialize(errResp);

                ctx.Response.WriteAsync(strErrResp);

                return Task.CompletedTask;
            }

            if (rowsAffected == 0)
            {
                ctx.Response.StatusCode = 400;

                DeleteUserResponse errResp = new DeleteUserResponse("User not exists");

                string strErrResp = JsonSerializer.Serialize(errResp);

                ctx.Response.WriteAsync(strErrResp);

                return Task.CompletedTask;
            }

            DeleteUserResponse resp = new DeleteUserResponse();

            string strResp = JsonSerializer.Serialize(resp);

            ctx.Response.StatusCode = 200;

            Task task = ctx.Response.WriteAsync(strResp);

            task.Wait();

            return Task.CompletedTask;
        }
    }
}
 