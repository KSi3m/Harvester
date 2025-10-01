using Harvester.Application.Dtos;
using Harvester.Application.Interfaces.OrderRules;
using Harvester.Domain.Models.Enums;
using Microsoft.IdentityModel.Tokens;
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
            var orders = dto.Combine!.Orders!
                .Where(x => DateOnly.FromDateTime(x.ScheduledDate) == dto.OrderDate)
                .Where(x => x.Status == OrderStatus.ACCEPTED)
                .Where(x => x.IsDeleted == false);
            
            if(orders.IsNullOrEmpty())
            {
                return new CheckRuleForOrderResponseDto
                {
                    Success = true,
                };
            }
            int alreadyAcceptedMinutes = 0;
            if(dto.OrderId != null)
            {
                alreadyAcceptedMinutes = orders.Where(x=>x.Id != dto.OrderId).Sum(x => x.EstimatedTime) + ((orders.Count()-1) * 15);
            }
            else
            {
                alreadyAcceptedMinutes = orders.Sum(x => x.EstimatedTime) + (orders.Count() * 15);
            }
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
