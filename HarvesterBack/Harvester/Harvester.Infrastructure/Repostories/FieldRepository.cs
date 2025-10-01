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
using Microsoft.AspNetCore.Mvc.Filters;

namespace Harvester.Infrastructure.Repostories
{
    public class FieldRepository(HarvesterDbContext dbContext) : IFieldRepository
    {
        public async Task CreateAsync(Field field)
        {
            await dbContext.Fields.AddAsync(field);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Field field)
        {
            dbContext.Fields.Attach(field);
            field.IsDeleted = true;
            var orders = field.Orders;
            foreach(var order in orders)
            {
                order.IsArchived = true;
            }
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Field>> GetAllAsync()
        {
            return await dbContext
                .Fields
                .AsNoTracking()
                .Where(x=> x.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<Field?> GetByIdAsync(int id, bool includeOrders = false)
        {
            var query = dbContext
                .Fields
                .AsNoTracking();

            if(includeOrders)
            {
                query = query.Include(x => x.Orders);
            }
                
            return await query
                .FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
        }

        public async Task UpdateAsync(Field field)
        {
            dbContext.Update(field);
            await dbContext.SaveChangesAsync();
        }
    }
}
