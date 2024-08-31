using API.Errors;

namespace API.MiddleWares
{
    public class ExceptionMiddleWare(RequestDelegate _next, ILogger<ExceptionMiddleWare> _logger,
        IHostEnvironment _env)
    {
        //httpcontext manages requests and responses 
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);//surround
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = _env.IsDevelopment() ? new ApiException((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace!.ToString())
                    : new ApiException((int)HttpStatusCode.InternalServerError);//help quickly identify the root cause 

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
