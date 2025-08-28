using Harvester.Application.Dtos;
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
        IFieldService fieldService
        ) : IOrderService
    {
        public async Task CreateAsync(CreateOrderDto dto)
        {
            var combine = await combineService.GetByIdAsync(dto.CombineId);
            var field = await fieldService.GetByIdAsync(dto.FieldId);

            //var travelTime = 15;


            var estimatedTime = (((field.AreaHectares / combine.BaseHaPerHour) / (field.ShapeCoeff * field.TerrainCoeff)) * 60); //+ travelTime;

            var info = new OrderInformationForCheck
            {
                FieldId = dto.FieldId,
                CombineId = dto.CombineId,
                OrderDate = dto.OrderDate,
                EstimatedTime = (int)estimatedTime,
            };
            
            if (await combineService.CheckAvailability(info))
            {
                var newOrder = new Order
                {
                    FieldId = dto.FieldId,
                    CombineId = dto.CombineId,
                    OrderDate = dto.OrderDate,
                    Status = OrderStatus.ACCEPTED, //po dodaniu panelu kombajnisty zmienić na pending
                    EstimatedTime = (int)estimatedTime,

                };
                await repository.CreateAsync(newOrder);
            }
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
