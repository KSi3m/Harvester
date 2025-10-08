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
        Task<IEnumerable<OrderDto>> GetAllAsync();
        Task<OrderDto?> GetByIdAsync(int id);

        Task<CheckRuleForOrderResponseDto> CreateAsync(CreateOrderDto dto);

        Task<CheckRuleForOrderResponseDto> UpdateAsync(int id, CreateOrderDto dto);
        Task DeleteAsync(int id);
        Task<CheckRuleForOrderResponseDto> CheckAvailability(OrderInformationForCheckAvailDto dto);

    }
}
