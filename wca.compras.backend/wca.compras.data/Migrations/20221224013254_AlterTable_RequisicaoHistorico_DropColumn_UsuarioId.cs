using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.compras.data.Migrations
{
    public partial class AlterTable_RequisicaoHistorico_DropColumn_UsuarioId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequisicaoHistoricos_Usuarios_usuario_id",
                table: "RequisicaoHistoricos");

            migrationBuilder.DropIndex(
                name: "IX_RequisicaoHistoricos_usuario_id",
                table: "RequisicaoHistoricos");

            migrationBuilder.DropColumn(
                name: "usuario_id",
                table: "RequisicaoHistoricos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "usuario_id",
                table: "RequisicaoHistoricos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RequisicaoHistoricos_usuario_id",
                table: "RequisicaoHistoricos",
                column: "usuario_id");

            migrationBuilder.AddForeignKey(
                name: "FK_RequisicaoHistoricos_Usuarios_usuario_id",
                table: "RequisicaoHistoricos",
                column: "usuario_id",
                principalTable: "Usuarios",
                principalColumn: "id");
        }
    }
}
