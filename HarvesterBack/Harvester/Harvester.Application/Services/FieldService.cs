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
    public class FieldService(IFieldRepository fieldRepository) : IFieldService
    {
        public async Task<IEnumerable<Field>> GetAll()
        {
            return await fieldRepository.GetAll();
        }

        public async Task<Field?> GetById(int id)
        {
            return await fieldRepository.GetById(id);
        }
    }
}
