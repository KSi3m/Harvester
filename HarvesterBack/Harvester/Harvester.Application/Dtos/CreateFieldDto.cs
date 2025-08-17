using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Application.Dtos
{
    public class CreateFieldDto
    {
        public string Name { get; set; }
        public decimal AreaHectares { get; set; }
        public decimal TerrainCoeff { get; set; } = 1.0m;
        public decimal ShapeCoeff { get; set; } = 1.0m;
        public string CropType { get; set; }
    }
}
