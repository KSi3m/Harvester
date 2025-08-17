using Harvester.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Infrastructure.Persistence
{
    public class HarvesterDbContext: DbContext
    {
        public DbSet<Field> Fields { get; set; }
        public DbSet<Combine> Combines { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public HarvesterDbContext(DbContextOptions<HarvesterDbContext> options) : base(options)
        {


        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(HarvesterDbContext).Assembly);
        }
    }
}
