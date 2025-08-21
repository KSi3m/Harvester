using Harvester.API.Application.ErrorResponse;
using Harvester.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace Harvester.API.Middlewares
{
    public class PlotNotFoundExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            if (exception is not PlotNotFoundOnOnGeoUtilityException plotNotFoundException)
            {
                return false;
            }

            var errorResponse = new ErrorResponse()
            {
                Status = StatusCodes.Status404NotFound,
                Message = "Not Found",
                Details = plotNotFoundException.Message
            };

            httpContext.Response.StatusCode = errorResponse.Status;

            await httpContext.Response
                .WriteAsJsonAsync(errorResponse, cancellationToken);

            return true;
        }
    }
}
