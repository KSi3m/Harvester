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

            modelBuilder.Entity<Field>()
                .HasMany(field => field.Orders)
                .WithOne(order => order.Field)
                .HasForeignKey(order => order.FieldId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Field>().HasKey(x => x.Id);

            modelBuilder.Entity<Field>()
                .Property(f => f.AreaHectares)
                .HasPrecision(5, 2);

            modelBuilder.Entity<Field>()
                .Property(f => f.Location)
                .HasMaxLength(200)
                .IsRequired(false);

            modelBuilder.Entity<Field>()
                .Property(f => f.CropType)
                .HasMaxLength(200);

            modelBuilder.Entity<Combine>().HasKey(x => x.Id);

            modelBuilder.Entity<Combine>()
                .Property(c => c.HeaderLength)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Combine>()
              .Property(c => c.Model)
              .HasMaxLength(100);

            modelBuilder.Entity<Combine>()
                .HasMany(c => c.Orders)
                .WithOne(o => o.Combine)
                .HasForeignKey(o => o.CombineId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Order>().HasKey(x=>x.Id);

            modelBuilder.Entity<Order>()
             .Property(o => o.Status)
             .HasMaxLength(20);

            modelBuilder.Entity<Order>()
                .Property(o => o.PricePerHectare)
                .HasPrecision(6, 2);

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalPrice)
                .HasPrecision(6, 2);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.Payments)
                .WithOne(p => p.Order)
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>().HasKey(x => x.Id);

            modelBuilder.Entity<Payment>()
                .Property(p=>p.Amount)
                .HasPrecision(6, 2);
        }
    }
}
