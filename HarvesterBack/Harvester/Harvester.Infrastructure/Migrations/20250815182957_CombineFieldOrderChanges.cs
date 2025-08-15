using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Harvester.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CombineFieldOrderChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Available",
                table: "Combines");

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "Fields",
                newName: "Name");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<decimal>(
                name: "ShapeCoeff",
                table: "Fields",
                type: "decimal(4,2)",
                precision: 4,
                scale: 2,
                nullable: false,
                defaultValue: 1m);

            migrationBuilder.AddColumn<decimal>(
                name: "TerrainCoeff",
                table: "Fields",
                type: "decimal(4,2)",
                precision: 4,
                scale: 2,
                nullable: false,
                defaultValue: 1m);

            migrationBuilder.AlterColumn<decimal>(
                name: "HeaderLength",
                table: "Combines",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AddColumn<decimal>(
                name: "AvailableWorkHours",
                table: "Combines",
                type: "decimal(4,2)",
                precision: 4,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BaseEfficency",
                table: "Combines",
                type: "decimal(4,2)",
                precision: 4,
                scale: 2,
                nullable: false,
                defaultValue: 0.75m);

            migrationBuilder.AddColumn<decimal>(
                name: "BaseHaPerHour",
                table: "Combines",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 1m);

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Combines",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.InsertData(
                table: "Combines",
                columns: new[] { "Id", "AvailableWorkHours", "BaseEfficency", "BaseHaPerHour", "HeaderLength", "IsAvailable", "Model" },
                values: new object[,]
                {
                    { 1, 11m, 0.75m, 2.5m, 6m, true, "John Deere X1" },
                    { 2, 11m, 0.75m, 3.0m, 7m, true, "Case IH 8230" }
                });

            migrationBuilder.InsertData(
                table: "Fields",
                columns: new[] { "Id", "AreaHectares", "CropType", "Name", "ShapeCoeff", "TerrainCoeff" },
                values: new object[,]
                {
                    { 1, 10.5m, "Wheat", "Pole A", 1.0m, 1.0m },
                    { 2, 8.2m, "Corn", "Pole B", 1.0m, 0.9m }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CombineId", "FieldId", "OrderDate", "PricePerHectare", "ScheduledDate", "Status", "TotalPrice" },
                values: new object[,]
                {
                    { 1, 1, 1, new DateTime(2025, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 150m, new DateTime(2025, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "ACCEPTED", 1575.0m },
                    { 2, 2, 2, new DateTime(2025, 8, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 140m, new DateTime(2025, 8, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "PENDING", 1148.0m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Combines",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Combines",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Fields",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Fields",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "ShapeCoeff",
                table: "Fields");

            migrationBuilder.DropColumn(
                name: "TerrainCoeff",
                table: "Fields");

            migrationBuilder.DropColumn(
                name: "AvailableWorkHours",
                table: "Combines");

            migrationBuilder.DropColumn(
                name: "BaseEfficency",
                table: "Combines");

            migrationBuilder.DropColumn(
                name: "BaseHaPerHour",
                table: "Combines");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Combines");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Fields",
                newName: "Location");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Orders",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<decimal>(
                name: "HeaderLength",
                table: "Combines",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldPrecision: 5,
                oldScale: 2);

            migrationBuilder.AddColumn<bool>(
                name: "Available",
                table: "Combines",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
