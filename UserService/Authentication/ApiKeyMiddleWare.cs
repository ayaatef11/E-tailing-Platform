
namespace Authentication;
public class ApiKeyMiddleware(RequestDelegate _next, IConfiguration _configuration)
{
 
    private const string ApiKeyHeaderName = "X-Api-Key"; // Customize your header name
   
    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var providedApiKey))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("API Key was not provided.");
            return;
        }

        var validApiKey = _configuration.GetValue<string>("ApiKey");
        if (!string.Equals(validApiKey, providedApiKey, StringComparison.Ordinal))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Invalid API Key.");
            return;
        }

        // Proceed to the next middleware in the pipeline
        await _next(context);
    }
}
