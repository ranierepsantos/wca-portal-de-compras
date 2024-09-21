using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.reembolso.infrastruture.Migrations
{
    /// <inheritdoc />
    public partial class AlterTable_Usuario_AddColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "cargo",
                table: "Usuarios",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "gestor_id",
                table: "Usuarios",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "perfil_id",
                table: "Usuarios",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cargo",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "gestor_id",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "perfil_id",
                table: "Usuarios");
        }
    }
}
