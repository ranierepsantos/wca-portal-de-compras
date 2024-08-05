using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.share.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateTable_Vagas_TabelasAuxiliares : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocumentoComplementar",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "varchar(200)", nullable: false),
                    ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentoComplementar", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Escalas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "varchar(200)", nullable: false),
                    ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Escalas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Escolaridade",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "varchar(200)", nullable: false),
                    ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Escolaridade", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Funcoes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "varchar(200)", nullable: false),
                    ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funcoes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Gestores",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "varchar(200)", nullable: false),
                    ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gestores", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Horarios",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "varchar(200)", nullable: false),
                    ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Horarios", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "MotivoContratacao",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "varchar(200)", nullable: false),
                    ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotivoContratacao", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Sexo",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "varchar(200)", nullable: false),
                    ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sexo", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "StatusVaga",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    status = table.Column<string>(type: "varchar(150)", nullable: false),
                    status_intermediario = table.Column<string>(type: "varchar(150)", nullable: false),
                    color = table.Column<string>(type: "varchar(30)", nullable: false),
                    notifica = table.Column<int>(type: "int", nullable: false),
                    autorizar = table.Column<bool>(type: "bit", nullable: false),
                    template_notificacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    proximo_status_id = table.Column<int>(type: "int", nullable: true, comment: "Utilizado quando campo autorizar = 1, após aprovação mudará para o status indicado aqui")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusVaga", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TipoFaturamento",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "varchar(200)", nullable: false),
                    ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoFaturamento", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TiposContrato",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "varchar(200)", nullable: false),
                    ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposContrato", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Vagas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cliente_id = table.Column<int>(type: "int", nullable: false),
                    tipocontrato_id = table.Column<int>(type: "int", nullable: false),
                    tipofaturamento_id = table.Column<int>(type: "int", nullable: false),
                    gestor_id = table.Column<int>(type: "int", nullable: false),
                    funcao_id = table.Column<int>(type: "int", nullable: false),
                    escala_id = table.Column<int>(type: "int", nullable: false),
                    horario_id = table.Column<int>(type: "int", nullable: false),
                    responsavel_id = table.Column<int>(type: "int", nullable: true),
                    data_solicitacao = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    status_vaga_id = table.Column<int>(type: "int", nullable: false),
                    quantidade_vagas = table.Column<int>(type: "int", nullable: false),
                    sexo_id = table.Column<int>(type: "int", nullable: false),
                    idade_minima = table.Column<int>(type: "int", nullable: false),
                    idade_maxima = table.Column<int>(type: "int", nullable: false),
                    caracteristica = table.Column<string>(type: "varchar(max)", nullable: true),
                    indicacao = table.Column<string>(type: "varchar(max)", nullable: true),
                    endereco_cliente = table.Column<string>(type: "varchar(1000)", nullable: true),
                    anotacoes = table.Column<string>(type: "varchar(max)", nullable: true),
                    motivocontratacao_id = table.Column<int>(type: "int", nullable: false),
                    justificativa_contratacao = table.Column<string>(type: "varchar(1000)", nullable: true),
                    permite_fumante = table.Column<bool>(type: "bit", nullable: false),
                    escolaridade_id = table.Column<int>(type: "int", nullable: false),
                    local_residencia = table.Column<string>(type: "varchar(1000)", nullable: true),
                    experiencia_profissional = table.Column<string>(type: "varchar(1000)", nullable: true),
                    descricao_atividade = table.Column<string>(type: "varchar(max)", nullable: true),
                    tem_cnh = table.Column<bool>(type: "bit", nullable: false),
                    cnh_categoria = table.Column<string>(type: "varchar(1)", nullable: true),
                    tem_vale_transporte = table.Column<bool>(type: "bit", nullable: false),
                    valor_vale_transporte = table.Column<decimal>(type: "money", nullable: true),
                    dias_transporte = table.Column<int>(type: "int", nullable: true),
                    refeicao = table.Column<string>(type: "varchar(200)", nullable: true),
                    outros_beneficios = table.Column<string>(type: "varchar(500)", nullable: true),
                    salario_base = table.Column<decimal>(type: "money", nullable: true),
                    tem_insalubridade = table.Column<bool>(type: "bit", nullable: false),
                    percentual_insalubridade = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    tem_periculosidade = table.Column<bool>(type: "bit", nullable: false),
                    percentual_periculosidade = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    data_inicio_prevista = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    tem_copia_admissao_cliente = table.Column<bool>(type: "bit", nullable: false),
                    tem_integracao_cliente = table.Column<bool>(type: "bit", nullable: false),
                    horario_integracao = table.Column<string>(type: "varchar(200)", nullable: true),
                    integracao_dias_semana = table.Column<string>(type: "varchar(30)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vagas", x => x.id);
                    table.ForeignKey(
                        name: "FK_Vagas_Clientes_cliente_id",
                        column: x => x.cliente_id,
                        principalTable: "Clientes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vagas_Escalas_escala_id",
                        column: x => x.escala_id,
                        principalTable: "Escalas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vagas_Escolaridade_escolaridade_id",
                        column: x => x.escolaridade_id,
                        principalTable: "Escolaridade",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vagas_Funcoes_funcao_id",
                        column: x => x.funcao_id,
                        principalTable: "Funcoes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vagas_Gestores_gestor_id",
                        column: x => x.gestor_id,
                        principalTable: "Gestores",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vagas_Horarios_horario_id",
                        column: x => x.horario_id,
                        principalTable: "Horarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vagas_MotivoContratacao_motivocontratacao_id",
                        column: x => x.motivocontratacao_id,
                        principalTable: "MotivoContratacao",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vagas_Sexo_sexo_id",
                        column: x => x.sexo_id,
                        principalTable: "Sexo",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vagas_StatusVaga_status_vaga_id",
                        column: x => x.status_vaga_id,
                        principalTable: "StatusVaga",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vagas_TipoFaturamento_tipofaturamento_id",
                        column: x => x.tipofaturamento_id,
                        principalTable: "TipoFaturamento",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vagas_TiposContrato_tipocontrato_id",
                        column: x => x.tipocontrato_id,
                        principalTable: "TiposContrato",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vagas_Usuarios_responsavel_id",
                        column: x => x.responsavel_id,
                        principalTable: "Usuarios",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "DocumentoComplementarVaga",
                columns: table => new
                {
                    DocumentoComplementaresId = table.Column<int>(type: "int", nullable: false),
                    VagaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentoComplementarVaga", x => new { x.DocumentoComplementaresId, x.VagaId });
                    table.ForeignKey(
                        name: "FK_DocumentoComplementarVaga_DocumentoComplementar_DocumentoComplementaresId",
                        column: x => x.DocumentoComplementaresId,
                        principalTable: "DocumentoComplementar",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentoComplementarVaga_Vagas_VagaId",
                        column: x => x.VagaId,
                        principalTable: "Vagas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VagaHistorico",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vaga_id = table.Column<int>(type: "int", nullable: false),
                    data_hora = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    evento = table.Column<string>(type: "varchar(500)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VagaHistorico", x => x.id);
                    table.ForeignKey(
                        name: "FK_VagaHistorico_Vagas_vaga_id",
                        column: x => x.vaga_id,
                        principalTable: "Vagas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentoComplementarVaga_VagaId",
                table: "DocumentoComplementarVaga",
                column: "VagaId");

            migrationBuilder.CreateIndex(
                name: "IX_VagaHistorico_vaga_id",
                table: "VagaHistorico",
                column: "vaga_id");

            migrationBuilder.CreateIndex(
                name: "IX_Vagas_cliente_id",
                table: "Vagas",
                column: "cliente_id");

            migrationBuilder.CreateIndex(
                name: "IX_Vagas_escala_id",
                table: "Vagas",
                column: "escala_id");

            migrationBuilder.CreateIndex(
                name: "IX_Vagas_escolaridade_id",
                table: "Vagas",
                column: "escolaridade_id");

            migrationBuilder.CreateIndex(
                name: "IX_Vagas_funcao_id",
                table: "Vagas",
                column: "funcao_id");

            migrationBuilder.CreateIndex(
                name: "IX_Vagas_gestor_id",
                table: "Vagas",
                column: "gestor_id");

            migrationBuilder.CreateIndex(
                name: "IX_Vagas_horario_id",
                table: "Vagas",
                column: "horario_id");

            migrationBuilder.CreateIndex(
                name: "IX_Vagas_motivocontratacao_id",
                table: "Vagas",
                column: "motivocontratacao_id");

            migrationBuilder.CreateIndex(
                name: "IX_Vagas_responsavel_id",
                table: "Vagas",
                column: "responsavel_id");

            migrationBuilder.CreateIndex(
                name: "IX_Vagas_sexo_id",
                table: "Vagas",
                column: "sexo_id");

            migrationBuilder.CreateIndex(
                name: "IX_Vagas_status_vaga_id",
                table: "Vagas",
                column: "status_vaga_id");

            migrationBuilder.CreateIndex(
                name: "IX_Vagas_tipocontrato_id",
                table: "Vagas",
                column: "tipocontrato_id");

            migrationBuilder.CreateIndex(
                name: "IX_Vagas_tipofaturamento_id",
                table: "Vagas",
                column: "tipofaturamento_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentoComplementarVaga");

            migrationBuilder.DropTable(
                name: "VagaHistorico");

            migrationBuilder.DropTable(
                name: "DocumentoComplementar");

            migrationBuilder.DropTable(
                name: "Vagas");

            migrationBuilder.DropTable(
                name: "Escalas");

            migrationBuilder.DropTable(
                name: "Escolaridade");

            migrationBuilder.DropTable(
                name: "Funcoes");

            migrationBuilder.DropTable(
                name: "Gestores");

            migrationBuilder.DropTable(
                name: "Horarios");

            migrationBuilder.DropTable(
                name: "MotivoContratacao");

            migrationBuilder.DropTable(
                name: "Sexo");

            migrationBuilder.DropTable(
                name: "StatusVaga");

            migrationBuilder.DropTable(
                name: "TipoFaturamento");

            migrationBuilder.DropTable(
                name: "TiposContrato");
        }
    }
}
