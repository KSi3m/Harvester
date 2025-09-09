using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace Harvester.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class geoJsonDataForField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Fields",
                newName: "CommonName");

            migrationBuilder.AddColumn<MultiPolygon>(
                name: "Boundary",
                table: "Fields",
                type: "geometry",
                nullable: true);

            migrationBuilder.AddColumn<Point>(
                name: "CenterPoint",
                table: "Fields",
                type: "geometry",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentifierName",
                table: "Fields",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Fields",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Boundary", "CenterPoint", "CommonName", "IdentifierName" },
                values: new object[] { null, null, null, "Pole A" });

            migrationBuilder.UpdateData(
                table: "Fields",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Boundary", "CenterPoint", "CommonName", "IdentifierName" },
                values: new object[] { null, null, null, "Pole B" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Boundary",
                table: "Fields");

            migrationBuilder.DropColumn(
                name: "CenterPoint",
                table: "Fields");

            migrationBuilder.DropColumn(
                name: "IdentifierName",
                table: "Fields");

            migrationBuilder.RenameColumn(
                name: "CommonName",
                table: "Fields",
                newName: "Name");

            migrationBuilder.UpdateData(
                table: "Fields",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Pole A");

            migrationBuilder.UpdateData(
                table: "Fields",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Pole B");
        }
    }
}
