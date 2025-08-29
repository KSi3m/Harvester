using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Domain.Models
{
    public class Field
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public decimal AreaHectares { get; set; }
        public decimal TerrainCoeff { get; set; } = 1.0m;
        public decimal ShapeCoeff { get; set; } = 1.0m;
        public string CropType { get; set; }

        public ICollection<Order>? Orders { get; set; }

      
    }
}
