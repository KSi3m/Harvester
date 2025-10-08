using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harvester.Domain.Models.Enums;

namespace Harvester.Domain.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int FieldId { get; set; }
        public Field? Field { get; set; }

        public int CombineId { get; set; }
        public Combine? Combine { get; set; }

        public DateTime OrderDate { get; set; }
        public DateTime ScheduledDate { get; set; }
        public OrderStatus Status { get; set; }
        public StrawProcessingMethod StrawProcessingMethod { get; set; }

        public int EstimatedTime { get; set; }
        public decimal EstimatedPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public ICollection<Payment>? Payments { get; set; }
        public bool IsDeleted { get; set; }

        public bool IsArchived { get; set; }
    }
}
