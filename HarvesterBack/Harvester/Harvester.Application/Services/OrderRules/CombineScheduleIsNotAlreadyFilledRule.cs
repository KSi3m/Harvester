using Harvester.Application.Dtos;
using Harvester.Application.Interfaces.OrderRules;
using Harvester.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Application.Services.OrderRules
{
    public class CombineScheduleIsNotAlreadyFilledRule : IOrderRule
    {
        public async Task<CheckRuleForOrderResponseDto> CheckRule(OrderInformationForCheckAvailDto dto)
        {
            var orders = dto.Combine!.Orders!.Where(x => x.OrderDate.Day == dto.OrderDate.Day && x.Status == OrderStatus.ACCEPTED);
            
            var alreadyAcceptedMinutes = orders.Sum(x => x.EstimatedTime) + (orders.Count() * 15);
            if (alreadyAcceptedMinutes + dto.EstimatedTime > dto.Combine.AvailableWorkHours * 60)
            {
                return new CheckRuleForOrderResponseDto
                {
                    Success = false,
                    RuleName = "Combine Schedule Is Already Filled",
                    Details = "You can't schedule any more orders for this day. Please try the next day"
                }; 
            }
            return new CheckRuleForOrderResponseDto
            {
                Success = true,   
            };
        }
    }
}
