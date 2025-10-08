using Harvester.Application.Dtos;
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
    public class CombineRepository(HarvesterDbContext dbContext) : ICombineRepository
    {
        public async Task<IEnumerable<Combine>> GetAllAsync()
        {
            return await dbContext
                .Combines
                .AsNoTracking()
                .Where(x => x.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<Combine?> GetByIdAsync(int id, bool includeOrders = false)
        {
            var query = dbContext
                .Combines
                .AsNoTracking();

            if (includeOrders)
            {
                query = query.Include(x => x.Orders);
            }
            return await query
                .FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
        }

        public async Task CreateAsync(Combine combine)
        {
            await dbContext.Combines.AddAsync(combine);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Combine combine)
        {
            dbContext.Combines.Attach(combine);
            combine.IsDeleted = true;
            var orders = combine.Orders;
            foreach (var order in orders)
            {
                order.IsArchived = true;
            }
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Combine combine)
        {
            dbContext.Update(combine);
            await dbContext.SaveChangesAsync();
        }

      
    }
}
