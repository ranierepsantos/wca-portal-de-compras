using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.reembolso.infrastruture.Migrations
{
    /// <inheritdoc />
    public partial class Inicial_Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    filial_id = table.Column<int>(type: "int", nullable: false),
                    nome = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    cnpj = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    inscricao_estadual = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    endereco = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true),
                    cep = table.Column<string>(type: "varchar(9)", maxLength: 9, nullable: true),
                    numero = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true),
                    cidade = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    uf = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: true),
                    ativo = table.Column<bool>(type: "bit", nullable: false),
                    valor_limite = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ContaCorrente",
                columns: table => new
                {
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    saldo = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContaCorrente", x => x.usuario_id);
                });

            migrationBuilder.CreateTable(
                name: "Notificacoes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    data_hora = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    nota = table.Column<string>(type: "varchar(500)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notificacoes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "StatusSolicitacao",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    texto = table.Column<string>(type: "varchar(150)", nullable: false),
                    color = table.Column<string>(type: "varchar(30)", nullable: false),
                    notifica = table.Column<int>(type: "int", nullable: false),
                    template_notificacao = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusSolicitacao", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TiposDespesa",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "varchar(50)", nullable: false),
                    ativo = table.Column<bool>(type: "bit", nullable: false),
                    tipo = table.Column<int>(type: "int", nullable: false),
                    valor = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposDespesa", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Faturamento",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataCriacao = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    cliente_id = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    Valor = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faturamento", x => x.id);
                    table.ForeignKey(
                        name: "FK_Faturamento_Clientes_cliente_id",
                        column: x => x.cliente_id,
                        principalTable: "Clientes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Solicitacoes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cliente_id = table.Column<int>(type: "int", nullable: false),
                    data_solicitacao = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    colaborador_id = table.Column<int>(type: "int", nullable: false),
                    gestor_id = table.Column<int>(type: "int", nullable: false),
                    colaborador_cargo = table.Column<string>(type: "varchar(100)", nullable: false),
                    projeto = table.Column<string>(type: "varchar(100)", nullable: false),
                    objetivo = table.Column<string>(type: "varchar(100)", nullable: false),
                    periodo_inicial = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    periodo_final = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    valor_adiantamento = table.Column<decimal>(type: "money", nullable: false),
                    valor_despesa = table.Column<decimal>(type: "money", nullable: false),
                    tipo_solicitacao = table.Column<int>(type: "int", nullable: false),
                    StatusSolicitacaoId = table.Column<int>(type: "int", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "UsuarioClientes",
                columns: table => new
                {
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    cliente_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioClientes", x => new { x.usuario_id, x.cliente_id });
                    table.ForeignKey(
                        name: "FK_UsuarioClientes_Clientes_cliente_id",
                        column: x => x.cliente_id,
                        principalTable: "Clientes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transacoes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    data_hora = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    descricao = table.Column<string>(type: "varchar(250)", nullable: false),
                    operador = table.Column<string>(type: "varchar(1)", nullable: false),
                    valor = table.Column<decimal>(type: "money", nullable: false),
                    ContaCorrenteUsuarioId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transacoes", x => x.id);
                    table.ForeignKey(
                        name: "FK_Transacoes_ContaCorrente_ContaCorrenteUsuarioId",
                        column: x => x.ContaCorrenteUsuarioId,
                        principalTable: "ContaCorrente",
                        principalColumn: "usuario_id");
                });

            migrationBuilder.CreateTable(
                name: "FaturamentoItems",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    faturamento_id = table.Column<int>(type: "int", nullable: false),
                    solicitacao_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaturamentoItems", x => x.id);
                    table.ForeignKey(
                        name: "FK_FaturamentoItems_Faturamento_faturamento_id",
                        column: x => x.faturamento_id,
                        principalTable: "Faturamento",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Despesas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    solicitacao_id = table.Column<int>(type: "int", nullable: false),
                    data_evento = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    tipodespesa_id = table.Column<int>(type: "int", nullable: false),
                    numero_fiscal = table.Column<string>(type: "varchar(100)", nullable: false),
                    valor = table.Column<decimal>(type: "money", nullable: false),
                    image_path = table.Column<string>(type: "varchar(200)", nullable: false),
                    razao_social = table.Column<string>(type: "varchar(150)", nullable: false),
                    cnpj = table.Column<string>(type: "varchar(20)", nullable: false),
                    inscricao_estadual = table.Column<string>(type: "varchar(20)", nullable: false),
                    motivo = table.Column<string>(type: "varchar(1000)", nullable: false),
                    origem = table.Column<string>(type: "varchar(1000)", nullable: false),
                    destino = table.Column<string>(type: "varchar(1000)", nullable: false),
                    km_percorrido = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Despesas", x => x.id);
                    table.ForeignKey(
                        name: "FK_Despesas_Solicitacoes_solicitacao_id",
                        column: x => x.solicitacao_id,
                        principalTable: "Solicitacoes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Despesas_TiposDespesa_tipodespesa_id",
                        column: x => x.tipodespesa_id,
                        principalTable: "TiposDespesa",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SolicitacaoHistorico",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    solicitacao_id = table.Column<int>(type: "int", nullable: false),
                    data_hora = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    evento = table.Column<string>(type: "varchar(500)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitacaoHistorico", x => x.id);
                    table.ForeignKey(
                        name: "FK_SolicitacaoHistorico_Solicitacoes_solicitacao_id",
                        column: x => x.solicitacao_id,
                        principalTable: "Solicitacoes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Despesas_solicitacao_id",
                table: "Despesas",
                column: "solicitacao_id");

            migrationBuilder.CreateIndex(
                name: "IX_Despesas_tipodespesa_id",
                table: "Despesas",
                column: "tipodespesa_id");

            migrationBuilder.CreateIndex(
                name: "IX_Faturamento_cliente_id",
                table: "Faturamento",
                column: "cliente_id");

            migrationBuilder.CreateIndex(
                name: "IX_FaturamentoItems_faturamento_id",
                table: "FaturamentoItems",
                column: "faturamento_id");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacaoHistorico_solicitacao_id",
                table: "SolicitacaoHistorico",
                column: "solicitacao_id");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacoes_cliente_id",
                table: "Solicitacoes",
                column: "cliente_id");

            migrationBuilder.CreateIndex(
                name: "IX_Transacoes_ContaCorrenteUsuarioId",
                table: "Transacoes",
                column: "ContaCorrenteUsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioClientes_cliente_id",
                table: "UsuarioClientes",
                column: "cliente_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Despesas");

            migrationBuilder.DropTable(
                name: "FaturamentoItems");

            migrationBuilder.DropTable(
                name: "Notificacoes");

            migrationBuilder.DropTable(
                name: "SolicitacaoHistorico");

            migrationBuilder.DropTable(
                name: "StatusSolicitacao");

            migrationBuilder.DropTable(
                name: "Transacoes");

            migrationBuilder.DropTable(
                name: "UsuarioClientes");

            migrationBuilder.DropTable(
                name: "TiposDespesa");

            migrationBuilder.DropTable(
                name: "Faturamento");

            migrationBuilder.DropTable(
                name: "Solicitacoes");

            migrationBuilder.DropTable(
                name: "ContaCorrente");

            migrationBuilder.DropTable(
                name: "Clientes");
        }
    }
}
