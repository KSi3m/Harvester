using Harvester.API.Application.ErrorResponse;
using Harvester.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

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

            var errorResponse = new ErrorResponse()
            {
                Status = StatusCodes.Status400BadRequest,
                Message = "Not Found",
                Details = modelStateException.Message
            };

            httpContext.Response.StatusCode = errorResponse.Status;

            await httpContext.Response
                .WriteAsJsonAsync(errorResponse, cancellationToken);

            return true;
        }
    }
}
