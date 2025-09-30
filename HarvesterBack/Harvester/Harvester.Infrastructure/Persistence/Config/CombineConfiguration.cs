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
    internal class CombineConfiguration : IEntityTypeConfiguration<Combine>
    {
        public void Configure(EntityTypeBuilder<Combine> modelBuilder)
        {

            modelBuilder.HasKey(x => x.Id);

            modelBuilder
                .Property(c => c.IsAvailable)
            .HasDefaultValue(true);

            modelBuilder
                .Property(c => c.HeaderLength)
            .HasPrecision(5, 2);

            modelBuilder
                .Property(c => c.BaseHaPerHour)
            .HasPrecision(5, 2)
            .HasDefaultValue(1);

            modelBuilder
                .Property(c => c.AvailableWorkHours)
            .HasPrecision(4, 2);

            modelBuilder
                .Property(c => c.BaseEfficency)
            .HasPrecision(4, 2)
            .HasDefaultValue(0.75m);

            modelBuilder
              .Property(c => c.Model)
            .HasMaxLength(100);

            modelBuilder
              .Property(c => c.HasStrawChopper)
            .HasDefaultValue(true);

            modelBuilder
              .Property(c => c.IsDeleted)
            .HasDefaultValue(false);

            modelBuilder
                .HasMany(c => c.Orders)
                .WithOne(o => o.Combine)
                .HasForeignKey(o => o.CombineId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.HasData(
            new Combine { Id = 1, Model = "John Deere X1", BaseHaPerHour = 2.5m, HeaderLength = 6m, IsAvailable = true, AvailableWorkHours = 11m, BaseEfficency = 0.75m, PricePerHectare = 600, },
            new Combine { Id = 2, Model = "Case IH 8230", BaseHaPerHour = 3.0m, HeaderLength = 7m, IsAvailable = true, AvailableWorkHours = 11m, BaseEfficency = 0.75m, PricePerHectare = 550, }
        );
        }
    }
}
