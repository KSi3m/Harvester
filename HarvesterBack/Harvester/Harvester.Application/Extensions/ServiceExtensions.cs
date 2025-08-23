using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harvester.Application.Interfaces.Services;
using Harvester.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using Harvester.Application.Dtos;
using Harvester.Application.Validators;
using FluentValidation.AspNetCore;

namespace Harvester.Application.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ICombineService, CombineService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IFieldService, FieldService>();
            services.AddScoped<IOnGeoService, OnGeoService>();

            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<CreateFieldDtoValidator>();
        }
    }
}
