using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.reembolso.infrastruture.Migrations
{
    /// <inheritdoc />
    public partial class AlterTable_FaturamentoItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_FaturamentoItems_solicitacao_id",
                table: "FaturamentoItems",
                column: "solicitacao_id");

            migrationBuilder.AddForeignKey(
                name: "FK_FaturamentoItems_Solicitacoes_solicitacao_id",
                table: "FaturamentoItems",
                column: "solicitacao_id",
                principalTable: "Solicitacoes",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FaturamentoItems_Solicitacoes_solicitacao_id",
                table: "FaturamentoItems");

            migrationBuilder.DropIndex(
                name: "IX_FaturamentoItems_solicitacao_id",
                table: "FaturamentoItems");
        }
    }
}
