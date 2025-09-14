using Harvester.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Application.Dtos
{
    public class OrderInformationForCheckAvailDto
    {
        public Field? Field { get; set; }
        public Combine? Combine { get; set; }

        public DateOnly OrderDate { get; set; }

        public int EstimatedTime { get; set; }

        public int? OrderId { get; set; }
    }
}
