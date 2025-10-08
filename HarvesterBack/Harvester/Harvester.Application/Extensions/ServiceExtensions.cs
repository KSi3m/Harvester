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
using Harvester.Application.Interfaces.OrderRules;
using Harvester.Application.Services.OrderRules;

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

            AddCheckRules(services);

            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<CreateFieldDtoValidator>();
        }

        public static void AddCheckRules(IServiceCollection services)
        {
            services.AddScoped<IOrderRule, CombineScheduleIsNotAlreadyFilledRule>();
            services.AddScoped<IOrderRule, FieldIsNotAlreadyOrderedRule>();
           // services.AddScoped<IOrderRule, FieldIsNotAlreadyHarvestedRule>();
        }
    }
}
