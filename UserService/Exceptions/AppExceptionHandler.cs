using Microsoft.AspNetCore.Diagnostics;

namespace UserService.Exceptions
{
    public class AppExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            await httpContext.Response.WriteAsJsonAsync("something went wrong ");
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            return true;
            //return ValueTask.FromResult(true);
        }
    }
}
