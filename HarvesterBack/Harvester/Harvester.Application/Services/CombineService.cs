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
    public class CombineService(ICombineRepository repository) : ICombineService
    {
        public async Task<IEnumerable<Combine>> GetAll()
        {
            return await repository.GetAll();
        }

        public async Task<Combine?> GetById(int id)
        {
            return await repository.GetById(id);
        }
    }
}
