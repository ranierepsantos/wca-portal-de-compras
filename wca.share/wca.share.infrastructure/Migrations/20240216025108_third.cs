using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.share.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class third : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemMudancaSolicitacaoMudancaBase_SolicitacaoMudancaBase_MudancaBasesSolicitacaoId",
                table: "ItemMudancaSolicitacaoMudancaBase");

            migrationBuilder.RenameColumn(
                name: "MudancaBasesSolicitacaoId",
                table: "ItemMudancaSolicitacaoMudancaBase",
                newName: "SolicitacaoMudancaBaseSolicitacaoId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemMudancaSolicitacaoMudancaBase_MudancaBasesSolicitacaoId",
                table: "ItemMudancaSolicitacaoMudancaBase",
                newName: "IX_ItemMudancaSolicitacaoMudancaBase_SolicitacaoMudancaBaseSolicitacaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemMudancaSolicitacaoMudancaBase_SolicitacaoMudancaBase_SolicitacaoMudancaBaseSolicitacaoId",
                table: "ItemMudancaSolicitacaoMudancaBase",
                column: "SolicitacaoMudancaBaseSolicitacaoId",
                principalTable: "SolicitacaoMudancaBase",
                principalColumn: "solicitacao_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemMudancaSolicitacaoMudancaBase_SolicitacaoMudancaBase_SolicitacaoMudancaBaseSolicitacaoId",
                table: "ItemMudancaSolicitacaoMudancaBase");

            migrationBuilder.RenameColumn(
                name: "SolicitacaoMudancaBaseSolicitacaoId",
                table: "ItemMudancaSolicitacaoMudancaBase",
                newName: "MudancaBasesSolicitacaoId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemMudancaSolicitacaoMudancaBase_SolicitacaoMudancaBaseSolicitacaoId",
                table: "ItemMudancaSolicitacaoMudancaBase",
                newName: "IX_ItemMudancaSolicitacaoMudancaBase_MudancaBasesSolicitacaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemMudancaSolicitacaoMudancaBase_SolicitacaoMudancaBase_MudancaBasesSolicitacaoId",
                table: "ItemMudancaSolicitacaoMudancaBase",
                column: "MudancaBasesSolicitacaoId",
                principalTable: "SolicitacaoMudancaBase",
                principalColumn: "solicitacao_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
