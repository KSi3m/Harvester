using Harvester.API.Application.ErrorResponse;
using Harvester.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Harvester.API.Middlewares
{
    public class NotFoundExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            if (exception is not NotFoundException notFoundException)
            {
                return false;
            }

            var errorResponse = new ProblemDetails
            {
                Title = "Not found",
                Detail = notFoundException.Message,
                Status = StatusCodes.Status404NotFound,
                Type = "https://httpstatuses.com/404",
                Instance = httpContext.Request.Path
            };

            httpContext.Response.StatusCode = errorResponse.Status.Value;

            await httpContext.Response
                .WriteAsJsonAsync(errorResponse, cancellationToken);

            return true;
        }
    }
}
