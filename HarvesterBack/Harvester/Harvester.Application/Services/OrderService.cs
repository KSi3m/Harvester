using Harvester.Application.Dtos;
using Harvester.Application.Exceptions;
using Harvester.Application.Interfaces.Repositories;
using Harvester.Application.Interfaces.Services;
using Harvester.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Application.Services
{
    public class OrderService(IOrderRepository repository, 
        ICombineService combineService,
        ICombineRepository combineRepository,
        IFieldRepository fieldRepository

        ) : IOrderService
    {
        public async Task<CheckRuleForOrderResponseDto> CreateAsync(CreateOrderDto dto)
        {
            var combine = await combineRepository.GetByIdAsync(dto.CombineId);
            var field = await fieldRepository.GetByIdAsync(dto.FieldId);

            if(combine == null) { throw new NotFoundException("Combine doesn't exist"); }
            if(field == null) { throw new NotFoundException("Field doesn't exist"); }
            
            var estimatedTime = (((field.AreaHectares / combine.BaseHaPerHour) / (field.ShapeCoeff * field.TerrainCoeff)) * 60); 

            var info = new OrderInformationForCheckAvailDto
            {
                Field = field,
                Combine = combine,
                OrderDate = DateOnly.FromDateTime(dto.OrderDate),
                EstimatedTime = (int)estimatedTime,
            };

            var res = await combineService.CheckAvailability(info);
            if (res.Success)
            {
                var newOrder = new Order
                {
                    FieldId = dto.FieldId,
                    CombineId = dto.CombineId,
                    OrderDate = dto.OrderDate,
                    ScheduledDate = dto.OrderDate, //po dodaniu panelu przy akceptacji kombajnista bedzie wybierał date
                    Status = OrderStatus.ACCEPTED, //po dodaniu panelu kombajnisty zmienić na pending
                    EstimatedTime = (int)estimatedTime,

                };
                await repository.CreateAsync(newOrder);
            }
            return res;
        }

        public async Task<IEnumerable<Order>> GetAll()
        {
            return await repository.GetAll();
        }

        public async Task<Order?> GetById(int id)
        {
            return await repository.GetById(id);
        }
    }
}
