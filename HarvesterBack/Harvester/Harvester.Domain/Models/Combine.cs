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
        public decimal HeaderLength { get; set; }
        public bool Available { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
