using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.compras.data.Migrations
{
    public partial class AlterTable_Perfil_AddColumn_SistemaId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "sistema_id",
                table: "Perfil",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Perfil_sistema_id",
                table: "Perfil",
                column: "sistema_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Perfil_Sistemas_sistema_id",
                table: "Perfil",
                column: "sistema_id",
                principalTable: "Sistemas",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Perfil_Sistemas_sistema_id",
                table: "Perfil");

            migrationBuilder.DropIndex(
                name: "IX_Perfil_sistema_id",
                table: "Perfil");

            migrationBuilder.DropColumn(
                name: "sistema_id",
                table: "Perfil");
        }
    }
}
