using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlquilerDeVehiculosApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRental : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VehicleId",
                table: "Rentals",
                newName: "SelectedVehicleTypeId");

            migrationBuilder.AlterColumn<int>(
                name: "PickedUpBranchId",
                table: "Rentals",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeliveredVehicleId",
                table: "Rentals",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveredVehicleId",
                table: "Rentals");

            migrationBuilder.RenameColumn(
                name: "SelectedVehicleTypeId",
                table: "Rentals",
                newName: "VehicleId");

            migrationBuilder.AlterColumn<int>(
                name: "PickedUpBranchId",
                table: "Rentals",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }
    }
}
