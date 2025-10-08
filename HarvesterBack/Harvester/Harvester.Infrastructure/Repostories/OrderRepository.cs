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
    public class OrderRepository(HarvesterDbContext dbContext) : IOrderRepository
    {
        public async Task CreateAsync(Order order)
        {
            await dbContext.Orders.AddAsync(order);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Order order)
        {
            dbContext.Orders.Attach(order);
            order.IsDeleted = true;
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await dbContext
                .Orders
                .Include(x=>x.Combine)
                .Include(x=>x.Field)
                .Where(x => x.IsDeleted == false)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetAllFromGivenYearAsync(int year)
        {
            return await dbContext
                .Orders
                .Include(x => x.Combine)
                .Include(x => x.Field)
                .AsNoTracking()
                .Where(x=>x.OrderDate.Year == year)
                .Where(x => x.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await dbContext
                .Orders
                .AsNoTracking()
                .Include(x => x.Combine)
                .Include(x => x.Field)
                .FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
        }

        public async Task UpdateAsync(Order order)
        {
            dbContext.Update(order);
            await dbContext.SaveChangesAsync();
        }
    }
}
