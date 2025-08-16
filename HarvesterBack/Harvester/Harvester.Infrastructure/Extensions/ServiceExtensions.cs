using Harvester.Application.Interfaces.Repositories;
using Harvester.Application.Interfaces.Services;
using Harvester.Application.Services;
using Harvester.Infrastructure.Persistence;
using Harvester.Infrastructure.Repostories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
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

            if (string.IsNullOrEmpty(configuration["OnGeoApi:BaseUrl"]))
                throw new Exception("OnGeoApi:BaseUrl is null!");
            var apiKey = configuration["OnGeoApi:ApiKey"];
            services.AddHttpClient<IOnGeoService, OnGeoService>(client =>
            {
                client.BaseAddress = new Uri(configuration["OnGeoApi:BaseUrl"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
                client.DefaultRequestHeaders.Add("Accept", "application/json, text/plain, */*");
                client.DefaultRequestHeaders.Add("Referer", "https://api.ongeo.pl/");
                client.DefaultRequestHeaders.Add("Origin", "https://api.ongeo.pl");
            });

            services.AddScoped<ICombineRepository,CombineRepository>();
            services.AddScoped<IOrderRepository,OrderRepository>();
            services.AddScoped<IFieldRepository,FieldRepository>();
        }
    }
}
