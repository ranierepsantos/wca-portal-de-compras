using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.share.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class seven : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SolicitacaoHistorico_solicitacao_id",
                table: "SolicitacaoHistorico",
                column: "solicitacao_id");

            migrationBuilder.AddForeignKey(
                name: "FK_SolicitacaoHistorico_Solicitacoes_solicitacao_id",
                table: "SolicitacaoHistorico",
                column: "solicitacao_id",
                principalTable: "Solicitacoes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacaoHistorico_data_hora",
                table: "SolicitacaoHistorico",
                column: "data_hora");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacao_data_solicitacao",
                table: "Solicitacoes",
                column: "data_solicitacao");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SolicitacaoHistorico_Solicitacoes_solicitacao_id",
                table: "SolicitacaoHistorico");

            migrationBuilder.DropIndex(
                name: "IX_SolicitacaoHistorico_solicitacao_id",
                table: "SolicitacaoHistorico");

            migrationBuilder.DropIndex(
                name: "IX_SolicitacaoHistorico_data_hora",
                table: "SolicitacaoHistorico");

            migrationBuilder.DropIndex(
                name: "IX_Solicitacao_data_solicitacao",
                table: "Solicitacoes");
        }
    }
}
