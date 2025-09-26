using Harvester.Application.Dtos;
using Harvester.Application.Exceptions;
using Harvester.Application.Interfaces.OrderRules;
using Harvester.Application.Interfaces.Repositories;
using Harvester.Application.Interfaces.Services;
using Harvester.Application.Mappings;
using Harvester.Domain.Models;
using Harvester.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Application.Services
{
    public class OrderService(IOrderRepository orderRepository, 
        ICombineRepository combineRepository,
        IFieldRepository fieldRepository,
        IEnumerable<IOrderRule> checkRules

        ) : IOrderService
    {
        public async Task<CheckRuleForOrderResponseDto> CreateAsync(CreateOrderDto dto)
        {
            var combine = await combineRepository.GetByIdAsync(dto.CombineId);
            if (combine == null)
            {
                throw new NotFoundException($"Combine with id: {dto.CombineId} doesn't exist");
            }

            var field = await fieldRepository.GetByIdAsync(dto.FieldId);
            if (field == null)
            {
                throw new NotFoundException($"Field with id: {dto.FieldId} doesn't exist");
            }

            var estimatedTime = (((field.AreaHectares / combine.BaseHaPerHour) / (field.ShapeCoeff * field.TerrainCoeff)) * 60);

            var info = new OrderInformationForCheckAvailDto
            {
                Field = field,
                Combine = combine,
                OrderDate = DateOnly.FromDateTime(dto.OrderDate),
                EstimatedTime = (int)estimatedTime,
                AllOrdersThisYear = await orderRepository.GetAllFromGivenYearAsync(dto.OrderDate.Year)
            };

            var res = await this.CheckAvailability(info);
            if (res.Success)
            {
                var priceForStrawProcessing = dto.StrawProcessingMethod == StrawProcessingMethod.CHOP ?  50 : 0;
                var estimatedPrice = field.AreaHectares * (combine.PricePerHectare + priceForStrawProcessing);
                var newOrder = new Order
                {
                    FieldId = dto.FieldId,
                    CombineId = dto.CombineId,
                    OrderDate = dto.OrderDate,
                    ScheduledDate = dto.OrderDate, //po dodaniu panelu przy akceptacji kombajnista bedzie wybierał date
                    Status = OrderStatus.ACCEPTED, //po dodaniu panelu kombajnisty zmienić na pending
                    EstimatedTime = (int)estimatedTime,
                    StrawProcessingMethod = dto.StrawProcessingMethod,
                    EstimatedPrice = estimatedPrice
                };
                await orderRepository.CreateAsync(newOrder);
            }
            return res;
        }

        public async Task DeleteAsync(int id)
        {
            var order = await orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                throw new NotFoundException("Order doesn't exist");
            }
            await orderRepository.DeleteAsync(order);
        }

        public async Task<IEnumerable<OrderDto>> GetAll()
        {
            var ordersDto = OrderMapppings.MapOrdersToOrderDtos(await orderRepository.GetAllAsync());
            return ordersDto;
        }

        public async Task<OrderDto?> GetById(int id)
        {
            var order = await orderRepository.GetByIdAsync(id);
            if(order == null)
            {
                throw new NotFoundException($"Order with id: {id} doesn't exist");
            }
            var orderDto = OrderMapppings.MapOrdertoOrderDto(order);
            return orderDto;
        }

        public async Task<CheckRuleForOrderResponseDto> UpdateAsync(int id, CreateOrderDto dto)
        {
            var order = await orderRepository.GetByIdAsync(id);
            if(order == null)
            {
                throw new NotFoundException($"Order with id: {id} doesn't exist");
            }

            var combine = await combineRepository.GetByIdAsync(dto.CombineId);
            if (combine == null)
            {
                throw new NotFoundException($"Combine with id: {id} doesn't exist");
            }

            var field = await fieldRepository.GetByIdAsync(dto.FieldId);
            if (field == null)
            {
                throw new NotFoundException($"Field with id: {id} doesn't exist");
            }

            var estimatedTime = (((field.AreaHectares / combine.BaseHaPerHour) / (field.ShapeCoeff * field.TerrainCoeff)) * 60);

            var info = new OrderInformationForCheckAvailDto
            {
                Field = field,
                Combine = combine,
                OrderDate = DateOnly.FromDateTime(dto.OrderDate),
                EstimatedTime = (int)estimatedTime,
                OrderId = id
            };

            var res = await this.CheckAvailability(info);
            if (res.Success)
            {
                var priceForStrawProcessing = dto.StrawProcessingMethod == StrawProcessingMethod.CHOP ? 50 : 0;
                var estimatedPrice = field.AreaHectares * (combine.PricePerHectare + priceForStrawProcessing);
  
                order.FieldId = dto.FieldId;
                order.CombineId = dto.CombineId;
                order.OrderDate = dto.OrderDate;
                order.ScheduledDate = dto.OrderDate; //po dodaniu panelu przy akceptacji kombajnista bedzie wybierał date
                order.Status = OrderStatus.ACCEPTED; //po dodaniu panelu kombajnisty zmienić na pending
                order.EstimatedTime = (int)estimatedTime;
                order.StrawProcessingMethod = dto.StrawProcessingMethod;
                order.EstimatedPrice = estimatedPrice;

                await orderRepository.UpdateAsync(order);
            }
            return res;
        }

        public async Task<CheckRuleForOrderResponseDto> CheckAvailability(OrderInformationForCheckAvailDto dto)
        {
            foreach (var rule in checkRules)
            {
                var res = await rule.CheckRule(dto);
                if (!res.Success) return res;
            }
            return new CheckRuleForOrderResponseDto { Success = true };
        }
    }
}
