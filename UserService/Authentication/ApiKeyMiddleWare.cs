using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private const string ApiKeyHeaderName = "X-Api-Key"; // Customize your header name
    private readonly IConfiguration _configuration;

    public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Check if the API key header is present in the request
        if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var providedApiKey))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("API Key was not provided.");
            return;
        }

        // Retrieve the valid API key from the configuration
        var validApiKey = _configuration.GetValue<string>("ApiKey");

        // Validate the API key
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
