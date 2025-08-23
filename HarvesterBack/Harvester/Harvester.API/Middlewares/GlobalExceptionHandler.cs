using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Harvester.API.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {

            var errorResponse = new ProblemDetails
            {
                Title = "Server error",
                Detail = exception.Message,
                Status = StatusCodes.Status500InternalServerError,
                Type = "https://httpstatuses.com/500",
                Instance = httpContext.Request.Path
            };

            httpContext.Response.StatusCode = errorResponse.Status.Value;

            await httpContext.Response
                .WriteAsJsonAsync(errorResponse, cancellationToken);

            return true;
        }
    }
}
