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
        //public int FieldId { get; set; }
        public Field? Field { get; set; }

       // public int CombineId { get; set; }
        public Combine? Combine { get; set; }

        public DateTime OrderDate { get; set; }

        public int EstimatedTime { get; set; }
    }
}
