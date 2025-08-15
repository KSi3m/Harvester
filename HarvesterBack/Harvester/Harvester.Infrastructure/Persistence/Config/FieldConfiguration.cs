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
                .Property(c => c.TerrainCoeff)
            .HasPrecision(4, 2)
            .HasDefaultValue(1m);

            modelBuilder
                .Property(c => c.ShapeCoeff)
            .HasPrecision(4, 2)
            .HasDefaultValue(1m);

            modelBuilder
                .Property(f => f.Name)
                .HasMaxLength(200)
            .IsRequired(false);

            modelBuilder
                .Property(f => f.CropType)
                .HasMaxLength(200);

            modelBuilder.HasData(
            new Field { Id = 1, Name = "Pole A", AreaHectares = 10.5m, TerrainCoeff = 1.0m, ShapeCoeff = 1.0m, CropType = "Wheat" },
            new Field { Id = 2, Name = "Pole B", AreaHectares = 8.2m, TerrainCoeff = 0.9m, ShapeCoeff = 1.0m, CropType = "Corn" }
            );

        }
    }
}
