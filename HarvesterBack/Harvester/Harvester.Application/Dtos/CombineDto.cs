using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Application.Dtos
{
    public class CombineDto
    {
        public int Id { get; set; }
        public string Model { get; set; }

        public decimal BaseHaPerHour { get; set; } 
        public decimal HeaderLength { get; set; }
        public bool IsAvailable { get; set; }
        public bool HasStrawChopper { get; set; }
        public int PricePerHectare { get; set; }
        public decimal AvailableWorkHours { get; set; } 

        public decimal BaseEfficency { get; set; } 
    }
}
