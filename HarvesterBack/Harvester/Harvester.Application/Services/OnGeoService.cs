using Harvester.Application.Dtos;
using Harvester.Application.Exceptions;
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
        public async Task<GeoServiceGetFieldDataResponse> GetDataAsync(string nameIdentifier)
        {
            var apiKey = configuration["OnGeoApi:ApiKey"];
            var baseUrl = configuration["OnGeoApi:BaseUrl"];

            var queryParams = new Dictionary<string, string>
            {
                ["api_key"] = apiKey!,
                ["query"] = Uri.EscapeDataString(nameIdentifier),
                ["additionalData"] = "boundary"
            };

            var url = QueryHelpers.AddQueryString(baseUrl!, queryParams!);

            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(jsonString);
            var root = doc.RootElement;

            if (root.ValueKind == JsonValueKind.Array && root.GetArrayLength() > 0)
            {

                var firstItem = root[0];

                var area = 0.0m;
                GeoMultiPolygonDto? boundary = null;
                GeoPointDto? point = null;

                firstItem.GetProperty("area").TryGetDecimal(out area);

                if (firstItem.TryGetProperty("boundary", out var boundaryJson))
                {
                    boundary = JsonSerializer.Deserialize<GeoMultiPolygonDto>(boundaryJson.GetRawText());
                }
                if (firstItem.TryGetProperty("point", out var pointJson))
                {
                    point = JsonSerializer.Deserialize<GeoPointDto>(pointJson.GetRawText());
                }

                return new GeoServiceGetFieldDataResponse
                {
                    AreaHectares = area / 10000,
                    CenterPoint = point,
                    Boundary = boundary
                };
            }
            throw new PlotNotFoundOnOnGeoUtilityException($"No area found for identifier {nameIdentifier}");
   
        }
    }
}
