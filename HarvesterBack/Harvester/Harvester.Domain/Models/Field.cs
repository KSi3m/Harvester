using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetTopologySuite.Geometries;

namespace Harvester.Domain.Models
{
    public class Field
    {
        public int Id { get; set; }
        public string IdentifierName { get; set; }

        public string? CommonName { get; set; }

        public decimal AreaHectares { get; set; }
        public decimal TerrainCoeff { get; set; } = 1.0m;
        public decimal ShapeCoeff { get; set; } = 1.0m;
        public string CropType { get; set; }

        public Point? CenterPoint { get; set; } = default!;

        public MultiPolygon? Boundary { get; set; } = default!;

        public ICollection<Order>? Orders { get; set; }

      
    }
}
