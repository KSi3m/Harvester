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
            return await dbContext.Combines.AsNoTracking().ToListAsync();
        }

        public async Task<Combine?> GetByIdAsync(int id)
        {
            return await dbContext.Combines.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task CreateAsync(Combine combine)
        {
            await dbContext.Combines.AddAsync(combine);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Combine combine)
        {
            dbContext.Combines.Remove(combine);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Combine combine)
        {
            dbContext.Update(combine);
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> CheckAvailability(OrderInformationForCheck dto)
        {
            var combine = await dbContext.Combines.AsNoTracking().Include(x=>x.Orders).FirstOrDefaultAsync(x => x.Id == dto.CombineId);
            var orders = combine.Orders.Where(x => x.OrderDate == dto.OrderDate && x.Status == OrderStatus.ACCEPTED);
            

            //zastanowić się nad przemodelowaniem pola żeby uwzględniać headerLength i szerokość pola

            if(orders.Any(x=>x.FieldId == dto.FieldId)) {
                return false; //pole już zamówione
            }
            var alreadyAcceptedMinutes = orders.Sum(x => x.EstimatedTime) + (orders.Count() * 15);
            if (alreadyAcceptedMinutes + dto.EstimatedTime > combine.AvailableWorkHours * 60)
            {
                return false; //kombajn już ma wypełniony dany dzień
            }

            return true;
        }
    }
}
