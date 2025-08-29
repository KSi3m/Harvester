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
    public class FieldIsNotAlreadyOrderedRule : IOrderRule
    {
        public async Task<CheckRuleForOrderResponseDto> CheckRule(OrderInformationForCheckAvailDto dto)
        {
            var orders = dto.Combine!.Orders!.Where(x => x.OrderDate.Day == dto.OrderDate.Day && x.Status == OrderStatus.ACCEPTED);
            //zastanowic się na tym czy to ma sens, bo przeciez to pole juz moglobyc wczesniej skoszone
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
                Success = false,
            };
        }
    }
}
