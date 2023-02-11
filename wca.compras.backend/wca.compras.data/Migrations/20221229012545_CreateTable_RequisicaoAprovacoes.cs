using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.compras.data.Migrations
{
    public partial class CreateTable_RequisicaoAprovacoes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RequisicaoAprovacoes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    requisicao_id = table.Column<int>(type: "int", nullable: false),
                    token = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    token_aprovador = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    data_criacao = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    data_expiracao = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    data_revogacao = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    nome_aprovador = table.Column<string>(type: "varchar(150)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequisicaoAprovacoes", x => x.id);
                    table.ForeignKey(
                        name: "FK_RequisicaoAprovacoes_Requisicoes_requisicao_id",
                        column: x => x.requisicao_id,
                        principalTable: "Requisicoes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequisicaoAprovacoes_requisicao_id",
                table: "RequisicaoAprovacoes",
                column: "requisicao_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequisicaoAprovacoes");
        }
    }
}
