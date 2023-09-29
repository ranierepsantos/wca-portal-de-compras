using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.reembolso.infrastruture.Migrations
{
    /// <inheritdoc />
    public partial class AlterTable_Notificacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Notificacoes_usuario_id",
                table: "Notificacoes",
                column: "usuario_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notificacoes_Usuarios_usuario_id",
                table: "Notificacoes",
                column: "usuario_id",
                principalTable: "Usuarios",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notificacoes_Usuarios_usuario_id",
                table: "Notificacoes");

            migrationBuilder.DropIndex(
                name: "IX_Notificacoes_usuario_id",
                table: "Notificacoes");
        }
    }
}
