using Harvester.Application.Dtos;
using Harvester.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Application.Interfaces.Services
{
    public interface ICombineService
    {
        Task<IEnumerable<CombineDto>> GetAllAsync();
        Task<CombineDto> GetByIdAsync(int id);
        Task CreateAsync(CreateCombineDto dto);
        Task UpdateAsync(int id, CreateCombineDto dto);
        Task DeleteAsync(int id);

        Task<bool> CheckAvailability(int id);
    }
}
