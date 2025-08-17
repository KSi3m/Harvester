using Harvester.Application.Dtos;
using Harvester.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Application.Interfaces.Services
{
    public interface IFieldService
    {
        Task<IEnumerable<Field>> GetAllAsync();
        Task<Field?> GetByIdAsync(int id);
        Task CreateAsync(CreateFieldDto dto);
        Task UpdateAsync(int id, CreateFieldDto dto);
        Task DeleteAsync(int id);
    }
}
