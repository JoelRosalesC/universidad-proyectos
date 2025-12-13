using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlquilerDeVehiculosApi.Migrations
{
    /// <inheritdoc />
    public partial class AddAdditionalsString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Additionals",
                table: "Rentals",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdditionalsList",
                table: "Rentals",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Additionals",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "AdditionalsList",
                table: "Rentals");
        }
    }
}
