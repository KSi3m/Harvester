using Harvester.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Application.Dtos
{
    public class CreateOrderDto
    {
        public int FieldId { get; set; }

        public int CombineId { get; set; }

        public DateTime OrderDate { get; set; }
    }
}
