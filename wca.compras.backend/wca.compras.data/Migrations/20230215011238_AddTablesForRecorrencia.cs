using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.compras.data.Migrations
{
    public partial class AddTablesForRecorrencia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Recorrencias",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    cliente_id = table.Column<int>(type: "int", nullable: false),
                    fornecedor_id = table.Column<int>(type: "int", nullable: false),
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    filial_id = table.Column<int>(type: "int", nullable: false),
                    tipo_recorrencia = table.Column<int>(type: "int", nullable: false),
                    dia = table.Column<int>(type: "int", nullable: false),
                    data_criacao = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    destino = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recorrencias", x => x.id);
                    table.ForeignKey(
                        name: "FK_Recorrencias_Clientes_cliente_id",
                        column: x => x.cliente_id,
                        principalTable: "Clientes",
                        principalColumn: "id"
                    );
                    table.ForeignKey(
                        name: "FK_Recorrencias_Filiais_filial_id",
                        column: x => x.filial_id,
                        principalTable: "Filiais",
                        principalColumn: "id"
                    );
                    table.ForeignKey(
                        name: "FK_Recorrencias_Fornecedores_fornecedor_id",
                        column: x => x.fornecedor_id,
                        principalTable: "Fornecedores",
                        principalColumn: "id"
                    );
                    table.ForeignKey(
                        name: "FK_Recorrencias_Usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "Usuarios",
                        principalColumn: "id"
                    );
                });

            migrationBuilder.CreateTable(
                name: "RecorrenciaLogs",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    recorrencia_id = table.Column<int>(type: "int", nullable: false),
                    data_hora = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    status = table.Column<string>(type: "varchar(15)", nullable: false),
                    log = table.Column<string>(type: "varchar(5000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecorrenciaLogs", x => x.id);
                    table.ForeignKey(
                        name: "FK_RecorrenciaLogs_Recorrencias_recorrencia_id",
                        column: x => x.recorrencia_id,
                        principalTable: "Recorrencias",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "RecorrenciaProdutos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    recorrencia_id = table.Column<int>(type: "int", nullable: false),
                    codigo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    unidade_medida = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    quantidade = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecorrenciaProdutos", x => x.id);
                    table.ForeignKey(
                        name: "FK_RecorrenciaProdutos_Recorrencias_recorrencia_id",
                        column: x => x.recorrencia_id,
                        principalTable: "Recorrencias",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecorrenciaLogs_recorrencia_id",
                table: "RecorrenciaLogs",
                column: "recorrencia_id");

            migrationBuilder.CreateIndex(
                name: "IX_RecorrenciaProdutos_recorrencia_id",
                table: "RecorrenciaProdutos",
                column: "recorrencia_id");

            migrationBuilder.CreateIndex(
                name: "IX_Recorrencias_cliente_id",
                table: "Recorrencias",
                column: "cliente_id");

            migrationBuilder.CreateIndex(
                name: "IX_Recorrencias_filial_id",
                table: "Recorrencias",
                column: "filial_id");

            migrationBuilder.CreateIndex(
                name: "IX_Recorrencias_fornecedor_id",
                table: "Recorrencias",
                column: "fornecedor_id");

            migrationBuilder.CreateIndex(
                name: "IX_Recorrencias_usuario_id",
                table: "Recorrencias",
                column: "usuario_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecorrenciaLogs");

            migrationBuilder.DropTable(
                name: "RecorrenciaProdutos");

            migrationBuilder.DropTable(
                name: "Recorrencias");
        }
    }
}
