using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Application.Dtos
{
    public class CheckRuleForOrderResponseDto
    {
        public bool Success { get; set; }
        public string? RuleName { get; set; }
        public string? Details { get; set; }

    }
}
