using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlquilerDeVehiculosApi.Migrations
{
    /// <inheritdoc />
    public partial class ChangeVehicleYear : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Year",
                table: "VehicleTypes");

            migrationBuilder.AddColumn<string>(
                name: "Year",
                table: "Vehicles",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Year",
                table: "Vehicles");

            migrationBuilder.AddColumn<DateTime>(
                name: "Year",
                table: "VehicleTypes",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
