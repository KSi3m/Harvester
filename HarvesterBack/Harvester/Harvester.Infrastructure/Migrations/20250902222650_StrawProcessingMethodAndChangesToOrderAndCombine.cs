using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Harvester.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class StrawProcessingMethodAndChangesToOrderAndCombine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PricePerHectare",
                table: "Orders",
                newName: "EstimatedPrice");

            migrationBuilder.AddColumn<string>(
                name: "StrawProcessingMethod",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "HasStrawChopper",
                table: "Combines",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "PricePerHectare",
                table: "Combines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Combines",
                keyColumn: "Id",
                keyValue: 1,
                column: "PricePerHectare",
                value: 600);

            migrationBuilder.UpdateData(
                table: "Combines",
                keyColumn: "Id",
                keyValue: 2,
                column: "PricePerHectare",
                value: 550);

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EstimatedPrice", "StrawProcessingMethod" },
                values: new object[] { 0m, "LEAVE" });

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EstimatedPrice", "StrawProcessingMethod" },
                values: new object[] { 0m, "CHOP" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StrawProcessingMethod",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "HasStrawChopper",
                table: "Combines");

            migrationBuilder.DropColumn(
                name: "PricePerHectare",
                table: "Combines");

            migrationBuilder.RenameColumn(
                name: "EstimatedPrice",
                table: "Orders",
                newName: "PricePerHectare");

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1,
                column: "PricePerHectare",
                value: 150m);

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 2,
                column: "PricePerHectare",
                value: 140m);
        }
    }
}
