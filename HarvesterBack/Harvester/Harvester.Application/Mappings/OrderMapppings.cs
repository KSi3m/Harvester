using Harvester.Application.Dtos;
using Harvester.Domain.Models;
using Harvester.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Application.Mappings
{
    public class OrderMapppings
    {
        public static OrderDto MapOrdertoOrderDto(Order order)
        {
            var orderDto = new OrderDto
            {
                Id = order.Id,
                FieldId = order.FieldId,
                CombineId = order.CombineId,
                OrderDate = order.OrderDate,
                ScheduledDate = order.ScheduledDate,
                Status = order.Status,
                StrawProcessingMethod = order.StrawProcessingMethod,
                EstimatedTime = order.EstimatedTime,
                EstimatedPrice = order.EstimatedPrice,
                TotalPrice = order.TotalPrice,
            };
            return orderDto;
        }
        public static IEnumerable<OrderDto> MapOrdersToOrderDtos(IEnumerable<Order> orders)
        {
            return orders.Select(MapOrdertoOrderDto);
        }
    }
}
