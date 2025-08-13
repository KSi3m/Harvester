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
    public class FieldConfiguration : IEntityTypeConfiguration<Field>
    {
        public void Configure(EntityTypeBuilder<Field> modelBuilder)
        {

            modelBuilder
                .HasMany(field => field.Orders)
                .WithOne(order => order.Field)
                .HasForeignKey(order => order.FieldId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.HasKey(x => x.Id);

            modelBuilder
                .Property(f => f.AreaHectares)
            .HasPrecision(5, 2);

            modelBuilder
                .Property(f => f.Location)
                .HasMaxLength(200)
            .IsRequired(false);

            modelBuilder
                .Property(f => f.CropType)
                .HasMaxLength(200);
        }
    }
}
