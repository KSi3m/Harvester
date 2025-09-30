using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Domain.Models
{
    public class Combine
    {
        public int Id { get; set; }
        public string Model { get; set; }

        public decimal BaseHaPerHour { get; set; } //ha per h 
        public decimal HeaderLength { get; set; }
        public bool IsAvailable { get; set; }
        public decimal AvailableWorkHours { get; set; } //dopisać że 11 h
        public bool HasStrawChopper { get; set; }

        public int PricePerHectare { get; set; }

        public decimal BaseEfficency { get; set; } //0.75

        public ICollection<Order>? Orders { get; set; }

        public bool IsDeleted { get; set; }
    }
}
