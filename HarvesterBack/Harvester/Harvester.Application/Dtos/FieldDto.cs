using Harvester.Application.Dtos;
using NetTopologySuite.Geometries;

namespace Harvester.Application
{
    public class FieldDto
    {
        public int Id { get; set; }
        public string IdentifierName { get; set; }
        public string? CommonName { get; set; }

        public decimal AreaHectares { get; set; }
        public decimal TerrainCoeff { get; set; } = 1.0m;
        public decimal ShapeCoeff { get; set; } = 1.0m;
        public string CropType { get; set; }

        public GeoPointDto? CenterPoint { get; set; } = default!;

        public GeoMultiPolygonDto? Boundary { get; set; } = default!;
    }
}
