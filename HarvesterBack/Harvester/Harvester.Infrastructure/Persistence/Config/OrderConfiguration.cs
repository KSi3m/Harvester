using Harvester.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Infrastructure.Persistence.Config
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> modelBuilder)
        {
            modelBuilder.HasKey(x => x.Id);

            modelBuilder
             .Property(o => o.Status)
            .HasMaxLength(20);

            modelBuilder
                .Property(o => o.PricePerHectare)
            .HasPrecision(6, 2);

            modelBuilder
                .Property(o => o.TotalPrice)
            .HasPrecision(6, 2);

            modelBuilder
                .HasMany(o => o.Payments)
                .WithOne(p => p.Order)
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
