using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Domain.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int FieldId { get; set; }
        public Field Field { get; set; }

        public int CombineId { get; set; }
        public Combine Combine { get; set; }

        public DateTime OrderDate { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string Status { get; set; }

        public decimal PricePerHectare { get; set; }
        public decimal TotalPrice { get; set; }
        public ICollection<Payment> Payments { get; set; }
    }
}
