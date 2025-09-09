using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Harvester.Application.Dtos
{
    public class GeoPointDto
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "Point";
        [JsonPropertyName("coordinates")]
        public double[] Coordinates { get; set; }
    }

    public class GeoMultiPolygonDto
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = "MultiPolygon";
        [JsonPropertyName("coordinates")]
        public double[][][][] Coordinates { get; set; } = Array.Empty<double[][][]>();
    }
}
