using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.share.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterTable_Solicitacao_RenameColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solicitacoes_SolicitacaoTipo_soliticacaotipo_id",
                table: "Solicitacoes");

            migrationBuilder.DropTable(
                name: "VagaHistorico");

            migrationBuilder.RenameColumn(
                name: "soliticacaotipo_id",
                table: "Solicitacoes",
                newName: "solicitacaotipo_id");

            migrationBuilder.RenameIndex(
                name: "IX_Solicitacoes_soliticacaotipo_id",
                table: "Solicitacoes",
                newName: "IX_Solicitacoes_solicitacaotipo_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitacoes_SolicitacaoTipo_solicitacaotipo_id",
                table: "Solicitacoes",
                column: "solicitacaotipo_id",
                principalTable: "SolicitacaoTipo",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solicitacoes_SolicitacaoTipo_solicitacaotipo_id",
                table: "Solicitacoes");

            migrationBuilder.RenameColumn(
                name: "solicitacaotipo_id",
                table: "Solicitacoes",
                newName: "soliticacaotipo_id");

            migrationBuilder.RenameIndex(
                name: "IX_Solicitacoes_solicitacaotipo_id",
                table: "Solicitacoes",
                newName: "IX_Solicitacoes_soliticacaotipo_id");

            migrationBuilder.CreateTable(
                name: "VagaHistorico",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    data_hora = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    evento = table.Column<string>(type: "varchar(500)", nullable: false),
                    SolicitacaoVagaSolicitacaoId = table.Column<int>(type: "int", nullable: true),
                    vaga_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VagaHistorico", x => x.id);
                    table.ForeignKey(
                        name: "FK_VagaHistorico_SolicitacaoVagas_SolicitacaoVagaSolicitacaoId",
                        column: x => x.SolicitacaoVagaSolicitacaoId,
                        principalTable: "SolicitacaoVagas",
                        principalColumn: "solicitacao_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_VagaHistorico_SolicitacaoVagaSolicitacaoId",
                table: "VagaHistorico",
                column: "SolicitacaoVagaSolicitacaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitacoes_SolicitacaoTipo_soliticacaotipo_id",
                table: "Solicitacoes",
                column: "soliticacaotipo_id",
                principalTable: "SolicitacaoTipo",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
