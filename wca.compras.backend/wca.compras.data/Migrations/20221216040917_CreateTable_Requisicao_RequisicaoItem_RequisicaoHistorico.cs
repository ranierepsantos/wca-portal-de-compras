using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.compras.data.Migrations
{
    public partial class CreateTable_Requisicao_RequisicaoItem_RequisicaoHistorico : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Requisicoes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    data_criacao = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    valor_total = table.Column<decimal>(type: "money", nullable: false),
                    taxa_gestao = table.Column<decimal>(type: "money", nullable: false),
                    status = table.Column<byte>(type: "tinyint", nullable: false),
                    destino = table.Column<byte>(type: "tinyint", nullable: false),
                    usuario_id = table.Column<int>(type: "int", nullable: true),
                    filial_id = table.Column<int>(type: "int", nullable: true),
                    cliente_id = table.Column<int>(type: "int", nullable: true),
                    fornecedor_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requisicoes", x => x.id);
                    table.ForeignKey(
                        name: "FK_Requisicoes_Clientes_cliente_id",
                        column: x => x.cliente_id,
                        principalTable: "Clientes",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Requisicoes_Filiais_filial_id",
                        column: x => x.filial_id,
                        principalTable: "Filiais",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Requisicoes_Fornecedores_fornecedor_id",
                        column: x => x.fornecedor_id,
                        principalTable: "Fornecedores",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Requisicoes_Usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "Usuarios",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "RequisicaoHistoricos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    requisicao_id = table.Column<int>(type: "int", nullable: false),
                    data_hora = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    evento = table.Column<string>(type: "varchar(500)", nullable: false),
                    usuario_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequisicaoHistoricos", x => x.id);
                    table.ForeignKey(
                        name: "FK_RequisicaoHistoricos_Requisicoes_requisicao_id",
                        column: x => x.requisicao_id,
                        principalTable: "Requisicoes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequisicaoHistoricos_Usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "Usuarios",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "RequisicaoItens",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    requisicao_id = table.Column<int>(type: "int", nullable: false),
                    produto_nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    unidade_medida = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    valor = table.Column<decimal>(type: "money", nullable: false),
                    taxa_gestao = table.Column<decimal>(type: "money", nullable: false),
                    quantidade = table.Column<int>(type: "int", nullable: false),
                    valor_total = table.Column<decimal>(type: "money", nullable: false),
                    tipofornecimento_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequisicaoItens", x => x.id);
                    table.ForeignKey(
                        name: "FK_RequisicaoItens_Requisicoes_requisicao_id",
                        column: x => x.requisicao_id,
                        principalTable: "Requisicoes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequisicaoItens_TipoFornecimento_tipofornecimento_id",
                        column: x => x.tipofornecimento_id,
                        principalTable: "TipoFornecimento",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequisicaoHistoricos_requisicao_id",
                table: "RequisicaoHistoricos",
                column: "requisicao_id");

            migrationBuilder.CreateIndex(
                name: "IX_RequisicaoHistoricos_usuario_id",
                table: "RequisicaoHistoricos",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_RequisicaoItens_requisicao_id",
                table: "RequisicaoItens",
                column: "requisicao_id");

            migrationBuilder.CreateIndex(
                name: "IX_RequisicaoItens_tipofornecimento_id",
                table: "RequisicaoItens",
                column: "tipofornecimento_id");

            migrationBuilder.CreateIndex(
                name: "IX_Requisicoes_cliente_id",
                table: "Requisicoes",
                column: "cliente_id");

            migrationBuilder.CreateIndex(
                name: "IX_Requisicoes_filial_id",
                table: "Requisicoes",
                column: "filial_id");

            migrationBuilder.CreateIndex(
                name: "IX_Requisicoes_fornecedor_id",
                table: "Requisicoes",
                column: "fornecedor_id");

            migrationBuilder.CreateIndex(
                name: "IX_Requisicoes_usuario_id",
                table: "Requisicoes",
                column: "usuario_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequisicaoHistoricos");

            migrationBuilder.DropTable(
                name: "RequisicaoItens");

            migrationBuilder.DropTable(
                name: "Requisicoes");
        }
    }
}
