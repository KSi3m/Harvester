using Harvester.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Application.Interfaces.Repositories
{
    public interface ICombineRepository
    {
        Task<IEnumerable<Combine>> GetAllAsync();
        Task<Combine?> GetByIdAsync(int id);
        Task CreateAsync(Combine combine);
        Task UpdateAsync(Combine combine);

        Task DeleteAsync(Combine combine);
    }
}
