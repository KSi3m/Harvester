using Harvester.Domain.Models;
using Harvester.Domain.Models.Enums;
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
            .HasConversion<string>();

            modelBuilder
                .Property(o => o.StrawProcessingMethod)
            .HasConversion<string>();

            modelBuilder
                .Property(o => o.TotalPrice)
            .HasPrecision(6, 2);

            modelBuilder
                .Property(o => o.EstimatedPrice)
            .HasPrecision(6, 2);

            modelBuilder
                .HasMany(o => o.Payments)
                .WithOne(p => p.Order)
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.HasData(
            new Order
            {
                Id = 1,
                FieldId = 1,
                CombineId = 1,
                OrderDate = new DateTime(2025, 8, 15),
                ScheduledDate = new DateTime(2025, 8, 20),
                Status = OrderStatus.ACCEPTED,
                TotalPrice = 150m * 10.5m,
                StrawProcessingMethod = StrawProcessingMethod.LEAVE
            },
            new Order
            {
                Id = 2,
                FieldId = 2,
                CombineId = 2,
                OrderDate = new DateTime(2025, 8, 16),
                ScheduledDate = new DateTime(2025, 8, 22),
                Status = OrderStatus.PENDING,
                TotalPrice = 140m * 8.2m,
                StrawProcessingMethod = StrawProcessingMethod.CHOP
            }
        );
        }
    }
}
