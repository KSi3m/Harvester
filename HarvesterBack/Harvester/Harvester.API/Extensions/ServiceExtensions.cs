using Harvester.API.Middlewares;
using Harvester.Application.Interfaces.Services;
using Harvester.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Harvester.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddMiddlewares(this IServiceCollection services)
        {
            services.AddConfiguration();
            services.AddExceptionHandler<ModelStateExceptionHandler>();
            services.AddExceptionHandler<PlotNotFoundExceptionHandler>();
            services.AddExceptionHandler<NotFoundExceptionHandler>();
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();
        }

        public static void AddConfiguration(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
        }
    }
}
