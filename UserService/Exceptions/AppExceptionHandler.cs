using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace UserService.Exceptions
{
    public class AppExceptionHandler(ILogger<AppExceptionHandler>logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError(exception, exception.Message);
            var details = new ProblemDetails()
            {
                Detail = $"Api error {exception.Message}",
                Instance = "API",
                Status = (int)HttpStatusCode.InternalServerError,
                Title = "Error",
                Type = "server error"
            };

            var response = JsonSerializer.Serialize(details);
            httpContext.Response.ContentType = "application/json";
            await httpContext.Response.WriteAsync(response/*JsonSerializer.Serialize(details)*/, cancellationToken);//.Wait();//write to the front  end the httpcontext response in the formate of json file
            return true;
        }
    }
}
