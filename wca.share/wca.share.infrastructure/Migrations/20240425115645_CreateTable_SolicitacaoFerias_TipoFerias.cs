using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.share.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateTable_SolicitacaoFerias_TipoFerias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TiposFerias",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    descricao = table.Column<string>(type: "varchar(100)", nullable: false),
                    quantidade_dias = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposFerias", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "SolicitacaoFerias",
                columns: table => new
                {
                    solicitacao_id = table.Column<int>(type: "int", nullable: false),
                    data_saida = table.Column<DateTime>(type: "datetime2", nullable: false),
                    data_retorno = table.Column<DateTime>(type: "datetime2", nullable: false),
                    tipoferias_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitacaoFerias", x => x.solicitacao_id);
                    table.ForeignKey(
                        name: "FK_SolicitacaoFerias_Solicitacoes_solicitacao_id",
                        column: x => x.solicitacao_id,
                        principalTable: "Solicitacoes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SolicitacaoFerias_TiposFerias_tipoferias_id",
                        column: x => x.tipoferias_id,
                        principalTable: "TiposFerias",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacaoFerias_tipoferias_id",
                table: "SolicitacaoFerias",
                column: "tipoferias_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SolicitacaoFerias");

            migrationBuilder.DropTable(
                name: "TiposFerias");
        }
    }
}
