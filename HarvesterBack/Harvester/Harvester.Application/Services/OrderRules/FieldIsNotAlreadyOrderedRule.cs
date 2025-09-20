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
    public class FieldIsNotAlreadyOrderedRule : IOrderRule
    {
        public async Task<CheckRuleForOrderResponseDto> CheckRule(OrderInformationForCheckAvailDto dto)
        {
            var trueResponse = new CheckRuleForOrderResponseDto
            {
                Success = true,
            };
            if (dto.AllOrdersThisYear.IsNullOrEmpty())
            {
                return trueResponse;
            }
            var orders = dto.AllOrdersThisYear!.Where(x => x.Status == OrderStatus.ACCEPTED);
            if(dto.OrderId != null && orders.Any(x => x.Id == dto.OrderId))
            {
                return trueResponse;
            }
            
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
