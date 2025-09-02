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
    public class FieldIsNotAlreadyOrderedRule : IOrderRule
    {
        public async Task<CheckRuleForOrderResponseDto> CheckRule(OrderInformationForCheckAvailDto dto)
        {
            var orders = dto.Combine!.Orders!.Where(x => DateOnly.FromDateTime(x.ScheduledDate) == dto.OrderDate && x.Status == OrderStatus.ACCEPTED);

            if (orders.Any(x => x.FieldId == dto.Field!.Id))
            {
                return new CheckRuleForOrderResponseDto
                {
                    Success = false,
                    RuleName = "Field is already ordered",
                    Details = "Given field is already ordered in different order"
                };
            }
            return new CheckRuleForOrderResponseDto
            {
                Success = true,
            };
        }
    }
}
