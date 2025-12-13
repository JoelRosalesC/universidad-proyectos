using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlquilerDeVehiculosApi.Migrations
{
    /// <inheritdoc />
    public partial class AddUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mail",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Salt",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Mail",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Salt",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Mail",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "Salt",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Admins");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Employees",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Customers",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Admins",
                newName: "UserId");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Mail = table.Column<string>(type: "TEXT", nullable: true),
                    Password = table.Column<string>(type: "TEXT", nullable: true),
                    Salt = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Role = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Employees",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Customers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Admins",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "Mail",
                table: "Employees",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Employees",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Salt",
                table: "Employees",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Employees",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Mail",
                table: "Customers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Customers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Salt",
                table: "Customers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Customers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Mail",
                table: "Admins",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Admins",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Salt",
                table: "Admins",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Admins",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
