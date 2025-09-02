using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Application.Dtos
{
    public class CreateCombineDto
    {
        public string Model { get; set; }
        public decimal BaseHaPerHour { get; set; } //ha per h 
        public decimal HeaderLength { get; set; }
        [DefaultValue(true)]
        public bool HasStrawChopper { get; set; }
        public int PricePerHectare { get; set; }
        public bool IsAvailable { get; set; }
        [DefaultValue(11)]
        public decimal AvailableWorkHours { get; set; } = 11m;
        [DefaultValue(0.75)]
        public decimal BaseEfficency { get; set; } = 0.75m;
    }
}
