using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.compras.data.Migrations
{
    public partial class AlterTable_Usuarios_DropColumn_PerfilId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Perfil_perfil_id",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_perfil_id",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "perfil_id",
                table: "Usuarios");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "perfil_id",
                table: "Usuarios",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_perfil_id",
                table: "Usuarios",
                column: "perfil_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Perfil_perfil_id",
                table: "Usuarios",
                column: "perfil_id",
                principalTable: "Perfil",
                principalColumn: "id");
        }
    }
}
