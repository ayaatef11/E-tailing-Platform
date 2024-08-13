using System.Text.RegularExpressions;

namespace WebApplication1.Authentication
{
    public class ApiKeyMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public ApiKeyMiddleWare(RequestDelegate next,IConfiguration configuration)
        {
            _next = next;
                _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
  var prodivedApiKey = context.Request.Headers[AuthConfig.ApiKeyHeader].FirstOrDefault();
            var isValid=IsValidApiKey(prodivedApiKey);
            if (isValid)
            {
                await GenerateResponse(context, 401, "Invalid Authentication");
                return;
            }
            await _next(context);
        }
        public bool IsValidApiKey(string providedApiKey)
        {
            if (string.IsNullOrEmpty(providedApiKey)) return false;
            var validApiKey = _configuration.GetValue<string>(AuthConfig.AuthSection);
            return string.Equals(validApiKey,providedApiKey,StringComparison.Ordinal);
        }
        public static async Task GenerateResponse(HttpContext context, int httpStatusCode,string msg)
        {
            context.Response.StatusCode = httpStatusCode;
            await context.Response.WriteAsync(msg);
        }
    }
}
