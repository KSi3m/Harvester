using Harvester.Application.Dtos;
using Harvester.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Application.Interfaces.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAll();
        Task<Order?> GetById(int id);

        Task<CheckRuleForOrderResponseDto> CreateAsync(CreateOrderDto dto);

    }
}
