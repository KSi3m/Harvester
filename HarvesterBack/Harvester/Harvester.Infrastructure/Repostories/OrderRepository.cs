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

        public async Task<IEnumerable<Order>> GetAll()
        {
            return await dbContext.Orders.Include(x=>x.Combine).Include(x=>x.Field).AsNoTracking().ToListAsync();
        }

        public async Task<Order?> GetById(int id)
        {
            return await dbContext.Orders.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
