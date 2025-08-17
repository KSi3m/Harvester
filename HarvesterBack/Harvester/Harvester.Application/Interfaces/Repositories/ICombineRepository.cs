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
        Task<IEnumerable<Combine>> GetAll();
        Task<Combine?> GetById(int id);
    }
}
