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
                .Property(c => c.HeaderLength)
            .HasPrecision(18, 2);

            modelBuilder
              .Property(c => c.Model)
            .HasMaxLength(100);

            modelBuilder
                .HasMany(c => c.Orders)
                .WithOne(o => o.Combine)
                .HasForeignKey(o => o.CombineId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
