using Harvester.Application.Interfaces.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Harvester.Application.Services
{
    public class OnGeoService(HttpClient httpClient, IConfiguration configuration) : IOnGeoService
    {
        public async Task<decimal> GetDataAsync(string nameIdentifier)
        {
            var apiKey = configuration["OnGeoApi:ApiKey"];
            var baseUrl = configuration["OnGeoApi:BaseUrl"];

            var queryParams = new Dictionary<string, string>
            {
                ["api_key"] = apiKey,
                ["query"] = Uri.EscapeDataString(nameIdentifier),
                ["additionalData"] = "boundary"
            };

            var url = QueryHelpers.AddQueryString(baseUrl, queryParams);

            Console.WriteLine(url);

            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(jsonString);
            var root = doc.RootElement;

            if (root.ValueKind == JsonValueKind.Array && root.GetArrayLength() > 0)
            {
                var firstItem = root[0];

                var area = 0.0m;

                firstItem.GetProperty("area").TryGetDecimal(out area);

                return area;
            }

            return 0.0m;
        }
    }
}
