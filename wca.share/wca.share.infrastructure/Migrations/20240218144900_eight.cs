using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.share.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class eight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StatusSolicitacaoId",
                table: "Solicitacoes",
                newName: "status_id");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacoes_status_id",
                table: "Solicitacoes",
                column: "status_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitacoes_StatusSolicitacao_status_id",
                table: "Solicitacoes",
                column: "status_id",
                principalTable: "StatusSolicitacao",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solicitacoes_StatusSolicitacao_status_id",
                table: "Solicitacoes");

            migrationBuilder.DropIndex(
                name: "IX_Solicitacoes_status_id",
                table: "Solicitacoes");

            migrationBuilder.RenameColumn(
                name: "status_id",
                table: "Solicitacoes",
                newName: "StatusSolicitacaoId");
        }
    }
}
