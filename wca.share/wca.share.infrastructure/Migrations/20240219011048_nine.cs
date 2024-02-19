using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.share.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class nine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solicitacoes_Funcionarios_responsavel_id",
                table: "Solicitacoes");

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitacoes_Usuarios_responsavel_id",
                table: "Solicitacoes",
                column: "responsavel_id",
                principalTable: "Usuarios",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solicitacoes_Usuarios_responsavel_id",
                table: "Solicitacoes");

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitacoes_Funcionarios_responsavel_id",
                table: "Solicitacoes",
                column: "responsavel_id",
                principalTable: "Funcionarios",
                principalColumn: "id");
        }
    }
}
