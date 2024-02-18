using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.share.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class second_addtables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assuntos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "varchar(200)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assuntos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Funcionarios",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    codigo_gi = table.Column<string>(type: "varchar(100)", nullable: false),
                    nome = table.Column<string>(type: "varchar(200)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funcionarios", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "MotivosDemissao",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    motivo = table.Column<string>(type: "varchar(300)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotivosDemissao", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "SolicitacaoTipo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tipo = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitacaoTipo", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Solicitacoes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    soliticacaotipo_id = table.Column<int>(type: "int", nullable: false),
                    cliente_id = table.Column<int>(type: "int", nullable: false),
                    funcionario_id = table.Column<int>(type: "int", nullable: false),
                    responsavel_id = table.Column<int>(type: "int", nullable: true),
                    gestor_id = table.Column<int>(type: "int", nullable: false),
                    data_solicitacao = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    descricao = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solicitacoes", x => x.id);
                    table.ForeignKey(
                        name: "FK_Solicitacoes_Clientes_cliente_id",
                        column: x => x.cliente_id,
                        principalTable: "Clientes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Solicitacoes_Funcionarios_funcionario_id",
                        column: x => x.funcionario_id,
                        principalTable: "Funcionarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Solicitacoes_Funcionarios_responsavel_id",
                        column: x => x.responsavel_id,
                        principalTable: "Funcionarios",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Solicitacoes_SolicitacaoTipo_soliticacaotipo_id",
                        column: x => x.soliticacaotipo_id,
                        principalTable: "SolicitacaoTipo",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Solicitacoes_Usuarios_gestor_id",
                        column: x => x.gestor_id,
                        principalTable: "Usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SolicitacaoComunicado",
                columns: table => new
                {
                    solicitacao_id = table.Column<int>(type: "int", nullable: false),
                    assunto_id = table.Column<int>(type: "int", nullable: false),
                    data_alteracao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    observacao = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitacaoComunicado", x => x.solicitacao_id);
                    table.ForeignKey(
                        name: "FK_SolicitacaoComunicado_Assuntos_assunto_id",
                        column: x => x.assunto_id,
                        principalTable: "Assuntos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SolicitacaoComunicado_Solicitacoes_solicitacao_id",
                        column: x => x.solicitacao_id,
                        principalTable: "Solicitacoes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SolicitacaoDesligamento",
                columns: table => new
                {
                    solicitacao_id = table.Column<int>(type: "int", nullable: false),
                    data_demissao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    motivodemissao_id = table.Column<int>(type: "int", nullable: false),
                    tem_contrato_experiencia = table.Column<bool>(type: "bit", nullable: false),
                    status_apontamento = table.Column<int>(type: "int", nullable: false),
                    status_aviso_previo = table.Column<int>(type: "int", nullable: false),
                    status_homologacao_sindicato = table.Column<int>(type: "int", nullable: false),
                    status_exame_demissional = table.Column<int>(type: "int", nullable: false),
                    data_credito = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitacaoDesligamento", x => x.solicitacao_id);
                    table.ForeignKey(
                        name: "FK_SolicitacaoDesligamento_MotivosDemissao_motivodemissao_id",
                        column: x => x.motivodemissao_id,
                        principalTable: "MotivosDemissao",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SolicitacaoDesligamento_Solicitacoes_solicitacao_id",
                        column: x => x.solicitacao_id,
                        principalTable: "Solicitacoes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SolicitacaoMudancaBase",
                columns: table => new
                {
                    solicitacao_id = table.Column<int>(type: "int", nullable: false),
                    data_alteracao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    observacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cliente_destino_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitacaoMudancaBase", x => x.solicitacao_id);
                    table.ForeignKey(
                        name: "FK_SolicitacaoMudancaBase_Clientes_cliente_destino_id",
                        column: x => x.cliente_destino_id,
                        principalTable: "Clientes",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_SolicitacaoMudancaBase_Solicitacoes_solicitacao_id",
                        column: x => x.solicitacao_id,
                        principalTable: "Solicitacoes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItensMudanca",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SolicitacaoMudancaBaseSolicitacaoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensMudanca", x => x.id);
                    table.ForeignKey(
                        name: "FK_ItensMudanca_SolicitacaoMudancaBase_SolicitacaoMudancaBaseSolicitacaoId",
                        column: x => x.SolicitacaoMudancaBaseSolicitacaoId,
                        principalTable: "SolicitacaoMudancaBase",
                        principalColumn: "solicitacao_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItensMudanca_SolicitacaoMudancaBaseSolicitacaoId",
                table: "ItensMudanca",
                column: "SolicitacaoMudancaBaseSolicitacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacaoComunicado_assunto_id",
                table: "SolicitacaoComunicado",
                column: "assunto_id");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacaoDesligamento_motivodemissao_id",
                table: "SolicitacaoDesligamento",
                column: "motivodemissao_id");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacaoMudancaBase_cliente_destino_id",
                table: "SolicitacaoMudancaBase",
                column: "cliente_destino_id");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacoes_cliente_id",
                table: "Solicitacoes",
                column: "cliente_id");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacoes_funcionario_id",
                table: "Solicitacoes",
                column: "funcionario_id");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacoes_gestor_id",
                table: "Solicitacoes",
                column: "gestor_id");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacoes_responsavel_id",
                table: "Solicitacoes",
                column: "responsavel_id");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacoes_soliticacaotipo_id",
                table: "Solicitacoes",
                column: "soliticacaotipo_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SolicitacaoComunicado");

            migrationBuilder.DropTable(
                name: "SolicitacaoDesligamento");

            migrationBuilder.DropTable(
                name: "SolicitacaoMudancaBase");

            migrationBuilder.DropTable(
                name: "Solicitacoes");

            migrationBuilder.DropTable(
                name: "Funcionarios");

            migrationBuilder.DropTable(
                name: "SolicitacaoTipo");

            migrationBuilder.DropTable(
                name: "ItensMudanca");

            migrationBuilder.DropTable(
                name: "Assuntos");

            migrationBuilder.DropTable(
                name: "MotivosDemissao");
        }
    }
}
