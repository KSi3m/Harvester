using Harvester.Application.Interfaces.Repositories;
using Harvester.Domain.Models;
using Harvester.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Harvester.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Infrastructure.Repostories
{
    public class FieldRepository(HarvesterDbContext dbContext) : IFieldRepository
    {
        public async Task CreateAsync(Field field)
        {
            await dbContext.Fields.AddAsync(field);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var field = await dbContext.Fields.FirstOrDefaultAsync(x=>x.Id == id);
            if (field == null) throw new NotFoundException("Field doesn't exist");
            dbContext.Fields.Remove(field);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Field>> GetAllAsync()
        {
            return await dbContext.Fields.AsNoTracking().ToListAsync();
        }

        public async Task<Field?> GetByIdAsync(int id)
        {
            return await dbContext.Fields.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(Field field)
        {
            dbContext.Update(field);
            await dbContext.SaveChangesAsync();
        }
    }
}
