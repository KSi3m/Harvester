namespace Harvester.Application
{
    public class FieldDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public decimal AreaHectares { get; set; }
        public decimal TerrainCoeff { get; set; } = 1.0m;
        public decimal ShapeCoeff { get; set; } = 1.0m;
        public string CropType { get; set; }
    }
}
