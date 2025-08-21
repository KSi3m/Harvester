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

            var errorResponse = new ErrorResponse()
            {
                Status = StatusCodes.Status404NotFound,
                Message = "Not Found",
                Details = notFoundException.Message
            };

            httpContext.Response.StatusCode = errorResponse.Status;

            await httpContext.Response
                .WriteAsJsonAsync(errorResponse, cancellationToken);

            return true;
        }
    }
}
