using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApplication1.Authentication
{
    public class ApiKeyAuthenticationFilter:IAsyncAuthorizationFilter
    {
        private readonly IConfiguration _configuration;
        public ApiKeyAuthenticationFilter(IConfiguration configuration   )
        {
            _configuration = configuration;   
        }
        public async  Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var providedKey = context.HttpContext.Request.Headers[AuthConfig.ApiKeyHeader].FirstOrDefault();
            var isValid= IsValidApiKey( providedKey );
            if (!isValid)
            {
                // Return 401 Unauthorized if the API key is invalid
                 context.Result =  new UnauthorizedObjectResult("Invalid Authentication");
                return;
            }
            // If valid, proceed with the request (do nothing here)
        }

        public bool IsValidApiKey(string providedApiKey)
        {
            if (string.IsNullOrEmpty(providedApiKey)) return false;

            var validApiKey = _configuration.GetValue<string>(AuthConfig.AuthSection);
            return string.Equals(validApiKey, providedApiKey, StringComparison.Ordinal);
        }

       public static async Task GenerateResponse(HttpContext context, int httpStatusCode, string msg)
        {
            context.Response.StatusCode = httpStatusCode;
            await context.Response.WriteAsync(msg);
        }
    }
}
