using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.share.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class third_changerelationship_itemmudanca : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItensMudanca_SolicitacaoMudancaBase_SolicitacaoMudancaBaseSolicitacaoId",
                table: "ItensMudanca");

            migrationBuilder.DropIndex(
                name: "IX_ItensMudanca_SolicitacaoMudancaBaseSolicitacaoId",
                table: "ItensMudanca");

            migrationBuilder.DropColumn(
                name: "SolicitacaoMudancaBaseSolicitacaoId",
                table: "ItensMudanca");

            migrationBuilder.CreateTable(
                name: "ItemMudancaSolicitacaoMudancaBase",
                columns: table => new
                {
                    ItensMudancaId = table.Column<int>(type: "int", nullable: false),
                    MudancaBasesSolicitacaoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemMudancaSolicitacaoMudancaBase", x => new { x.ItensMudancaId, x.MudancaBasesSolicitacaoId });
                    table.ForeignKey(
                        name: "FK_ItemMudancaSolicitacaoMudancaBase_ItensMudanca_ItensMudancaId",
                        column: x => x.ItensMudancaId,
                        principalTable: "ItensMudanca",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemMudancaSolicitacaoMudancaBase_SolicitacaoMudancaBase_MudancaBasesSolicitacaoId",
                        column: x => x.MudancaBasesSolicitacaoId,
                        principalTable: "SolicitacaoMudancaBase",
                        principalColumn: "solicitacao_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemMudancaSolicitacaoMudancaBase_MudancaBasesSolicitacaoId",
                table: "ItemMudancaSolicitacaoMudancaBase",
                column: "MudancaBasesSolicitacaoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemMudancaSolicitacaoMudancaBase");

            migrationBuilder.AddColumn<int>(
                name: "SolicitacaoMudancaBaseSolicitacaoId",
                table: "ItensMudanca",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItensMudanca_SolicitacaoMudancaBaseSolicitacaoId",
                table: "ItensMudanca",
                column: "SolicitacaoMudancaBaseSolicitacaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItensMudanca_SolicitacaoMudancaBase_SolicitacaoMudancaBaseSolicitacaoId",
                table: "ItensMudanca",
                column: "SolicitacaoMudancaBaseSolicitacaoId",
                principalTable: "SolicitacaoMudancaBase",
                principalColumn: "solicitacao_id");
        }
    }
}
