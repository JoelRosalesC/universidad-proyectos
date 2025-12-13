using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlquilerDeVehiculosApi.Migrations
{
    /// <inheritdoc />
    public partial class AddIdInVehicleEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Vehicles",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Brand",
                table: "VehicleTypes");

            migrationBuilder.AddColumn<int>(
                name: "BrandId",
                table: "VehicleTypes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "LicensePlate",
                table: "Vehicles",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Vehicles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vehicles",
                table: "Vehicles",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vehicles",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "VehicleTypes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Vehicles");

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "VehicleTypes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LicensePlate",
                table: "Vehicles",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vehicles",
                table: "Vehicles",
                column: "LicensePlate");
        }
    }
}
