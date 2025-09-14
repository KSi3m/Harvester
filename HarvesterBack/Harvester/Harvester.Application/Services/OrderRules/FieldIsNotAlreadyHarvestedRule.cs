using Harvester.Application.Dtos;
using Harvester.Application.Interfaces.OrderRules;
using Harvester.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Application.Services.OrderRules
{
    public class FieldIsNotAlreadyHarvestedRule : IOrderRule
    {
        public async Task<CheckRuleForOrderResponseDto> CheckRule(OrderInformationForCheckAvailDto dto)
        {
            var orders = dto.Combine!.Orders!.Where(x => x.ScheduledDate.Year == dto.OrderDate.Year && x.Status == OrderStatus.FINISHED);
 
            if (orders.Any(x => x.FieldId == dto.Field!.Id))
            {
                return new CheckRuleForOrderResponseDto
                {
                    Success = false,
                    RuleName = "Field is already harvested",
                    Details = "Given field was already harvested"
                };
            }
            return new CheckRuleForOrderResponseDto
            {
                Success = true,
            };
        }
    }
}
