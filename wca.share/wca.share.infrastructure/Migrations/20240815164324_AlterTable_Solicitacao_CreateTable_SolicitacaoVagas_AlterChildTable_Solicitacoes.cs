using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.share.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterTable_Solicitacao_CreateTable_SolicitacaoVagas_AlterChildTable_Solicitacoes : Migration
    {
        /// <inheritdoc />
        
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solicitacoes_CentrosDeCusto_centrocusto_id",
                table: "Solicitacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Solicitacoes_Funcionarios_funcionario_id",
                table: "Solicitacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_VagaHistorico_Vagas_vaga_id",
                table: "VagaHistorico");

            migrationBuilder.DropTable(
                name: "DocumentoComplementarVaga");

            migrationBuilder.DropTable(
                name: "Vagas");

            migrationBuilder.DropIndex(
                name: "IX_VagaHistorico_vaga_id",
                table: "VagaHistorico");

            migrationBuilder.DropIndex(
                name: "IX_Solicitacoes_centrocusto_id",
                table: "Solicitacoes");

            migrationBuilder.DropIndex(
                name: "IX_Solicitacoes_funcionario_id",
                table: "Solicitacoes");

            migrationBuilder.AddColumn<int>(
                name: "centrocusto_id",
                table: "SolicitacaoMudancaBase",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "funcionario_id",
                table: "SolicitacaoMudancaBase",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "centrocusto_id",
                table: "SolicitacaoFerias",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "funcionario_id",
                table: "SolicitacaoFerias",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "centrocusto_id",
                table: "SolicitacaoDesligamento",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "funcionario_id",
                table: "SolicitacaoDesligamento",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "centrocusto_id",
                table: "SolicitacaoComunicado",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "funcionario_id",
                table: "SolicitacaoComunicado",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SolicitacaoVagas",
                columns: table => new
                {
                    solicitacao_id = table.Column<int>(type: "int", nullable: false),
                    tipocontrato_id = table.Column<int>(type: "int", nullable: false),
                    tipofaturamento_id = table.Column<int>(type: "int", nullable: false),
                    gestor_id = table.Column<int>(type: "int", nullable: false),
                    funcao_id = table.Column<int>(type: "int", nullable: false),
                    escala_id = table.Column<int>(type: "int", nullable: false),
                    horario_id = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_SolicitacaoVagas", x => x.solicitacao_id);
                    table.ForeignKey(
                        name: "FK_SolicitacaoVagas_Escalas_escala_id",
                        column: x => x.escala_id,
                        principalTable: "Escalas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SolicitacaoVagas_Escolaridade_escolaridade_id",
                        column: x => x.escolaridade_id,
                        principalTable: "Escolaridade",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SolicitacaoVagas_Funcoes_funcao_id",
                        column: x => x.funcao_id,
                        principalTable: "Funcoes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SolicitacaoVagas_Gestores_gestor_id",
                        column: x => x.gestor_id,
                        principalTable: "Gestores",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SolicitacaoVagas_Horarios_horario_id",
                        column: x => x.horario_id,
                        principalTable: "Horarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SolicitacaoVagas_MotivoContratacao_motivocontratacao_id",
                        column: x => x.motivocontratacao_id,
                        principalTable: "MotivoContratacao",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SolicitacaoVagas_Sexo_sexo_id",
                        column: x => x.sexo_id,
                        principalTable: "Sexo",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SolicitacaoVagas_Solicitacoes_solicitacao_id",
                        column: x => x.solicitacao_id,
                        principalTable: "Solicitacoes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SolicitacaoVagas_TipoFaturamento_tipofaturamento_id",
                        column: x => x.tipofaturamento_id,
                        principalTable: "TipoFaturamento",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SolicitacaoVagas_TiposContrato_tipocontrato_id",
                        column: x => x.tipocontrato_id,
                        principalTable: "TiposContrato",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentoComplementarSolicitacaoVaga",
                columns: table => new
                {
                    DocumentoComplementaresId = table.Column<int>(type: "int", nullable: false),
                    SolicitacaoVagaSolicitacaoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentoComplementarSolicitacaoVaga", x => new { x.DocumentoComplementaresId, x.SolicitacaoVagaSolicitacaoId });
                    table.ForeignKey(
                        name: "FK_DocumentoComplementarSolicitacaoVaga_DocumentoComplementar_DocumentoComplementaresId",
                        column: x => x.DocumentoComplementaresId,
                        principalTable: "DocumentoComplementar",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentoComplementarSolicitacaoVaga_SolicitacaoVagas_SolicitacaoVagaSolicitacaoId",
                        column: x => x.SolicitacaoVagaSolicitacaoId,
                        principalTable: "SolicitacaoVagas",
                        principalColumn: "solicitacao_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacaoMudancaBase_centrocusto_id",
                table: "SolicitacaoMudancaBase",
                column: "centrocusto_id");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacaoMudancaBase_funcionario_id",
                table: "SolicitacaoMudancaBase",
                column: "funcionario_id");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacaoFerias_centrocusto_id",
                table: "SolicitacaoFerias",
                column: "centrocusto_id");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacaoFerias_funcionario_id",
                table: "SolicitacaoFerias",
                column: "funcionario_id");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacaoDesligamento_centrocusto_id",
                table: "SolicitacaoDesligamento",
                column: "centrocusto_id");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacaoDesligamento_funcionario_id",
                table: "SolicitacaoDesligamento",
                column: "funcionario_id");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacaoComunicado_centrocusto_id",
                table: "SolicitacaoComunicado",
                column: "centrocusto_id");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacaoComunicado_funcionario_id",
                table: "SolicitacaoComunicado",
                column: "funcionario_id");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentoComplementarSolicitacaoVaga_SolicitacaoVagaSolicitacaoId",
                table: "DocumentoComplementarSolicitacaoVaga",
                column: "SolicitacaoVagaSolicitacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacaoVagas_escala_id",
                table: "SolicitacaoVagas",
                column: "escala_id");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacaoVagas_escolaridade_id",
                table: "SolicitacaoVagas",
                column: "escolaridade_id");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacaoVagas_funcao_id",
                table: "SolicitacaoVagas",
                column: "funcao_id");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacaoVagas_gestor_id",
                table: "SolicitacaoVagas",
                column: "gestor_id");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacaoVagas_horario_id",
                table: "SolicitacaoVagas",
                column: "horario_id");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacaoVagas_motivocontratacao_id",
                table: "SolicitacaoVagas",
                column: "motivocontratacao_id");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacaoVagas_sexo_id",
                table: "SolicitacaoVagas",
                column: "sexo_id");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacaoVagas_tipocontrato_id",
                table: "SolicitacaoVagas",
                column: "tipocontrato_id");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacaoVagas_tipofaturamento_id",
                table: "SolicitacaoVagas",
                column: "tipofaturamento_id");

            migrationBuilder.AddForeignKey(
                name: "FK_SolicitacaoComunicado_CentrosDeCusto_centrocusto_id",
                table: "SolicitacaoComunicado",
                column: "centrocusto_id",
                principalTable: "CentrosDeCusto",
                principalColumn: "id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_SolicitacaoComunicado_Funcionarios_funcionario_id",
                table: "SolicitacaoComunicado",
                column: "funcionario_id",
                principalTable: "Funcionarios",
                principalColumn: "id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_SolicitacaoDesligamento_CentrosDeCusto_centrocusto_id",
                table: "SolicitacaoDesligamento",
                column: "centrocusto_id",
                principalTable: "CentrosDeCusto",
                principalColumn: "id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_SolicitacaoDesligamento_Funcionarios_funcionario_id",
                table: "SolicitacaoDesligamento",
                column: "funcionario_id",
                principalTable: "Funcionarios",
                principalColumn: "id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_SolicitacaoFerias_CentrosDeCusto_centrocusto_id",
                table: "SolicitacaoFerias",
                column: "centrocusto_id",
                principalTable: "CentrosDeCusto",
                principalColumn: "id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_SolicitacaoFerias_Funcionarios_funcionario_id",
                table: "SolicitacaoFerias",
                column: "funcionario_id",
                principalTable: "Funcionarios",
                principalColumn: "id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_SolicitacaoMudancaBase_CentrosDeCusto_centrocusto_id",
                table: "SolicitacaoMudancaBase",
                column: "centrocusto_id",
                principalTable: "CentrosDeCusto",
                principalColumn: "id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_SolicitacaoMudancaBase_Funcionarios_funcionario_id",
                table: "SolicitacaoMudancaBase",
                column: "funcionario_id",
                principalTable: "Funcionarios",
                principalColumn: "id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.Sql(@"UPDATE up SET
                centrocusto_id = so.centrocusto_id,
                funcionario_id = so.funcionario_id 
             FROM SolicitacaoComunicado up 
             INNER JOIN Solicitacoes so ON so.id = up.solicitacao_id");

            migrationBuilder.Sql(@"UPDATE up SET
                centrocusto_id = so.centrocusto_id,
                funcionario_id = so.funcionario_id 
             FROM SolicitacaoDesligamento up 
             INNER JOIN Solicitacoes so ON so.id = up.solicitacao_id");

            migrationBuilder.Sql(@"UPDATE up SET
                centrocusto_id = so.centrocusto_id,
                funcionario_id = so.funcionario_id 
             FROM SolicitacaoFerias up 
             INNER JOIN Solicitacoes so ON so.id = up.solicitacao_id");

            migrationBuilder.Sql(@"UPDATE up SET
                centrocusto_id = so.centrocusto_id,
                funcionario_id = so.funcionario_id 
             FROM SolicitacaoMudancaBase up 
             INNER JOIN Solicitacoes so ON so.id = up.solicitacao_id");

            migrationBuilder.DropColumn(
                name: "centrocusto_id",
                table: "Solicitacoes");

            migrationBuilder.DropColumn(
                name: "funcionario_id",
                table: "Solicitacoes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "centrocusto_id",
                table: "Solicitacoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "funcionario_id",
                table: "Solicitacoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(@"UPDATE so SET
                centrocusto_id = up.centrocusto_id,
                funcionario_id = up.funcionario_id 
             FROM Solicitacoes so  
             INNER JOIN SolicitacaoComunicado up ON so.id = up.solicitacao_id");

            migrationBuilder.Sql(@"UPDATE so SET
                centrocusto_id = up.centrocusto_id,
                funcionario_id = up.funcionario_id 
             FROM Solicitacoes so  
             INNER JOIN SolicitacaoDesligamento up ON so.id = up.solicitacao_id");

            migrationBuilder.Sql(@"UPDATE so SET
                centrocusto_id = up.centrocusto_id,
                funcionario_id = up.funcionario_id 
             FROM Solicitacoes so  
             INNER JOIN SolicitacaoFerias up ON so.id = up.solicitacao_id");

            migrationBuilder.Sql(@"UPDATE so SET
                centrocusto_id = up.centrocusto_id,
                funcionario_id = up.funcionario_id 
             FROM Solicitacoes so  
             INNER JOIN SolicitacaoMudancaBase up ON so.id = up.solicitacao_id");


            migrationBuilder.DropForeignKey(
                name: "FK_SolicitacaoComunicado_CentrosDeCusto_centrocusto_id",
                table: "SolicitacaoComunicado");

            migrationBuilder.DropForeignKey(
                name: "FK_SolicitacaoComunicado_Funcionarios_funcionario_id",
                table: "SolicitacaoComunicado");

            migrationBuilder.DropForeignKey(
                name: "FK_SolicitacaoDesligamento_CentrosDeCusto_centrocusto_id",
                table: "SolicitacaoDesligamento");

            migrationBuilder.DropForeignKey(
                name: "FK_SolicitacaoDesligamento_Funcionarios_funcionario_id",
                table: "SolicitacaoDesligamento");

            migrationBuilder.DropForeignKey(
                name: "FK_SolicitacaoFerias_CentrosDeCusto_centrocusto_id",
                table: "SolicitacaoFerias");

            migrationBuilder.DropForeignKey(
                name: "FK_SolicitacaoFerias_Funcionarios_funcionario_id",
                table: "SolicitacaoFerias");

            migrationBuilder.DropForeignKey(
                name: "FK_SolicitacaoMudancaBase_CentrosDeCusto_centrocusto_id",
                table: "SolicitacaoMudancaBase");

            migrationBuilder.DropForeignKey(
                name: "FK_SolicitacaoMudancaBase_Funcionarios_funcionario_id",
                table: "SolicitacaoMudancaBase");

            
            migrationBuilder.DropTable(
                name: "DocumentoComplementarSolicitacaoVaga");

            migrationBuilder.DropTable(
                name: "SolicitacaoVagas");

            migrationBuilder.DropIndex(
                name: "IX_SolicitacaoMudancaBase_centrocusto_id",
                table: "SolicitacaoMudancaBase");

            migrationBuilder.DropIndex(
                name: "IX_SolicitacaoMudancaBase_funcionario_id",
                table: "SolicitacaoMudancaBase");

            migrationBuilder.DropIndex(
                name: "IX_SolicitacaoFerias_centrocusto_id",
                table: "SolicitacaoFerias");

            migrationBuilder.DropIndex(
                name: "IX_SolicitacaoFerias_funcionario_id",
                table: "SolicitacaoFerias");

            migrationBuilder.DropIndex(
                name: "IX_SolicitacaoDesligamento_centrocusto_id",
                table: "SolicitacaoDesligamento");

            migrationBuilder.DropIndex(
                name: "IX_SolicitacaoDesligamento_funcionario_id",
                table: "SolicitacaoDesligamento");

            migrationBuilder.DropIndex(
                name: "IX_SolicitacaoComunicado_centrocusto_id",
                table: "SolicitacaoComunicado");

            migrationBuilder.DropIndex(
                name: "IX_SolicitacaoComunicado_funcionario_id",
                table: "SolicitacaoComunicado");

            migrationBuilder.DropColumn(
                name: "centrocusto_id",
                table: "SolicitacaoMudancaBase");

            migrationBuilder.DropColumn(
                name: "funcionario_id",
                table: "SolicitacaoMudancaBase");

            migrationBuilder.DropColumn(
                name: "centrocusto_id",
                table: "SolicitacaoFerias");

            migrationBuilder.DropColumn(
                name: "funcionario_id",
                table: "SolicitacaoFerias");

            migrationBuilder.DropColumn(
                name: "centrocusto_id",
                table: "SolicitacaoDesligamento");

            migrationBuilder.DropColumn(
                name: "funcionario_id",
                table: "SolicitacaoDesligamento");

            migrationBuilder.DropColumn(
                name: "centrocusto_id",
                table: "SolicitacaoComunicado");

            migrationBuilder.DropColumn(
                name: "funcionario_id",
                table: "SolicitacaoComunicado");

            migrationBuilder.CreateTable(
                name: "Vagas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cliente_id = table.Column<int>(type: "int", nullable: false),
                    escala_id = table.Column<int>(type: "int", nullable: false),
                    escolaridade_id = table.Column<int>(type: "int", nullable: false),
                    funcao_id = table.Column<int>(type: "int", nullable: false),
                    gestor_id = table.Column<int>(type: "int", nullable: false),
                    horario_id = table.Column<int>(type: "int", nullable: false),
                    motivocontratacao_id = table.Column<int>(type: "int", nullable: false),
                    responsavel_id = table.Column<int>(type: "int", nullable: true),
                    sexo_id = table.Column<int>(type: "int", nullable: false),
                    status_solicitacao_id = table.Column<int>(type: "int", nullable: false),
                    tipocontrato_id = table.Column<int>(type: "int", nullable: false),
                    tipofaturamento_id = table.Column<int>(type: "int", nullable: false),
                    anotacoes = table.Column<string>(type: "varchar(max)", nullable: true),
                    caracteristica = table.Column<string>(type: "varchar(max)", nullable: true),
                    cnh_categoria = table.Column<string>(type: "varchar(1)", nullable: true),
                    data_inicio_prevista = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    data_solicitacao = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    descricao_atividade = table.Column<string>(type: "varchar(max)", nullable: true),
                    dias_transporte = table.Column<int>(type: "int", nullable: true),
                    endereco_cliente = table.Column<string>(type: "varchar(1000)", nullable: true),
                    experiencia_profissional = table.Column<string>(type: "varchar(1000)", nullable: true),
                    horario_integracao = table.Column<string>(type: "varchar(200)", nullable: true),
                    idade_maxima = table.Column<int>(type: "int", nullable: false),
                    idade_minima = table.Column<int>(type: "int", nullable: false),
                    indicacao = table.Column<string>(type: "varchar(max)", nullable: true),
                    integracao_dias_semana = table.Column<string>(type: "varchar(30)", nullable: true),
                    justificativa_contratacao = table.Column<string>(type: "varchar(1000)", nullable: true),
                    local_residencia = table.Column<string>(type: "varchar(1000)", nullable: true),
                    outros_beneficios = table.Column<string>(type: "varchar(500)", nullable: true),
                    percentual_insalubridade = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    percentual_periculosidade = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    permite_fumante = table.Column<bool>(type: "bit", nullable: false),
                    quantidade_vagas = table.Column<int>(type: "int", nullable: false),
                    refeicao = table.Column<string>(type: "varchar(200)", nullable: true),
                    salario_base = table.Column<decimal>(type: "money", nullable: true),
                    tem_cnh = table.Column<bool>(type: "bit", nullable: false),
                    tem_copia_admissao_cliente = table.Column<bool>(type: "bit", nullable: false),
                    tem_insalubridade = table.Column<bool>(type: "bit", nullable: false),
                    tem_integracao_cliente = table.Column<bool>(type: "bit", nullable: false),
                    tem_periculosidade = table.Column<bool>(type: "bit", nullable: false),
                    tem_vale_transporte = table.Column<bool>(type: "bit", nullable: false),
                    valor_vale_transporte = table.Column<decimal>(type: "money", nullable: true)
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
                        name: "FK_Vagas_StatusSolicitacao_status_solicitacao_id",
                        column: x => x.status_solicitacao_id,
                        principalTable: "StatusSolicitacao",
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

            migrationBuilder.CreateIndex(
                name: "IX_VagaHistorico_vaga_id",
                table: "VagaHistorico",
                column: "vaga_id");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacoes_centrocusto_id",
                table: "Solicitacoes",
                column: "centrocusto_id");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacoes_funcionario_id",
                table: "Solicitacoes",
                column: "funcionario_id");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentoComplementarVaga_VagaId",
                table: "DocumentoComplementarVaga",
                column: "VagaId");

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
                name: "IX_Vagas_status_solicitacao_id",
                table: "Vagas",
                column: "status_solicitacao_id");

            migrationBuilder.CreateIndex(
                name: "IX_Vagas_tipocontrato_id",
                table: "Vagas",
                column: "tipocontrato_id");

            migrationBuilder.CreateIndex(
                name: "IX_Vagas_tipofaturamento_id",
                table: "Vagas",
                column: "tipofaturamento_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitacoes_CentrosDeCusto_centrocusto_id",
                table: "Solicitacoes",
                column: "centrocusto_id",
                principalTable: "CentrosDeCusto",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitacoes_Funcionarios_funcionario_id",
                table: "Solicitacoes",
                column: "funcionario_id",
                principalTable: "Funcionarios",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VagaHistorico_Vagas_vaga_id",
                table: "VagaHistorico",
                column: "vaga_id",
                principalTable: "Vagas",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
