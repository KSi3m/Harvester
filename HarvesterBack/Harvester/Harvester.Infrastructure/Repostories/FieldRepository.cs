using Harvester.Application.Interfaces.Repositories;
using Harvester.Domain.Models;
using Harvester.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Infrastructure.Repostories
{
    public class FieldRepository(HarvesterDbContext dbContext) : IFieldRepository
    {
        public async Task<IEnumerable<Field>> GetAll()
        {
            return await dbContext.Fields.AsNoTracking().ToListAsync();
        }

        public async Task<Field?> GetById(int id)
        {
            return await dbContext.Fields.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
