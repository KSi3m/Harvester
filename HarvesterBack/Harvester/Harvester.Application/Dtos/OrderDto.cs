using Harvester.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Harvester.Application.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int FieldId { get; set; }
        public int CombineId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ScheduledDate { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrderStatus Status { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public StrawProcessingMethod StrawProcessingMethod { get; set; }
        public int EstimatedTime { get; set; }
        public decimal EstimatedPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
