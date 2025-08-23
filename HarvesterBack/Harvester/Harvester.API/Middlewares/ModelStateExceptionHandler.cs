using Harvester.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Harvester.API.Middlewares
{
    public class ModelStateExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            if (exception is not ModelStateException modelStateException)
            {
                return false;
            }

            var errorResponse = new ProblemDetails
            {
                Title = "Bad request",
                Detail = modelStateException.Message,
                Status = StatusCodes.Status400BadRequest,
                Type = "https://httpstatuses.com/400",
                Instance = httpContext.Request.Path
            };

            httpContext.Response.StatusCode = errorResponse.Status.Value;

            await httpContext.Response
                .WriteAsJsonAsync(errorResponse, cancellationToken);

            return true;
        }
    }
}
