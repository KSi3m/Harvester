using Harvester.API.Application.ErrorResponse;
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

            var errorResponse = new ErrorResponse()
            {
                Status = StatusCodes.Status500InternalServerError,
                Message = "Server error",
                Details = "Error occured"
            };

            httpContext.Response.StatusCode = errorResponse.Status;

            await httpContext.Response
                .WriteAsJsonAsync(errorResponse, cancellationToken);

            return true;
        }
    }
}
