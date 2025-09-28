using NetTopologySuite.Geometries;
using NetTopologySuite.IO.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Harvester.Application.Dtos
{
    public class CreateFieldDto
    {
        public string IdentifierName { get; set; }
        public string? CommonName { get; set; }
        public decimal AreaHectares { get; set; }
        public decimal? TerrainCoeff { get; set; } = 1.0m;
        public decimal? ShapeCoeff { get; set; } = 1.0m;
        public string CropType { get; set; }

        public GeoPointDto CenterPoint { get; set; } = default!;

        public GeoMultiPolygonDto Boundary { get; set; } = default!;
    }
}
