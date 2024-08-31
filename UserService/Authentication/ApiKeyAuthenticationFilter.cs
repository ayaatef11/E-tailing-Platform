using UserService.DTOs.Configuration;

namespace Authentication
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
                 context.Result =  new UnauthorizedObjectResult("Invalid Authentication");
               
            }
        }

        private  bool IsValidApiKey(string providedApiKey)
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
