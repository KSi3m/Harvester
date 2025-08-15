using Harvester.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Application.Interfaces.Repositories
{
    public interface IFieldRepository
    {
        Task<IEnumerable<Field>> GetAll();
        Task<Field?> GetById(int id);
    }
}
