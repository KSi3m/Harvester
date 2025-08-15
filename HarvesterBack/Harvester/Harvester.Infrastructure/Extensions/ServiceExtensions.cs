using Harvester.Application.Interfaces.Repositories;
using Harvester.Infrastructure.Persistence;
using Harvester.Infrastructure.Repostories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Infrastructure.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("HarvesterDb");

            services.AddDbContext<HarvesterDbContext>(options => options.UseSqlServer(connectionString));

            services.AddScoped<ICombineRepository,CombineRepository>();
            services.AddScoped<IOrderRepository,OrderRepository>();
            services.AddScoped<IFieldRepository,FieldRepository>();
        }
    }
}
