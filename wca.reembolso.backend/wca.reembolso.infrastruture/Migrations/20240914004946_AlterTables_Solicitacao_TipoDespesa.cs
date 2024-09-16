using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.reembolso.infrastruture.Migrations
{
    /// <inheritdoc />
    public partial class AlterTables_Solicitacao_TipoDespesa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "exibir_para_colaborador",
                table: "TiposDespesa",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "faturar_cliente",
                table: "TiposDespesa",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "reembolsar_colaborador",
                table: "TiposDespesa",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "objetivo",
                table: "Solicitacoes",
                type: "varchar(1000)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)");

            migrationBuilder.AddColumn<DateTime>(
                name: "data_entrega",
                table: "Solicitacoes",
                type: "smalldatetime",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "data_prevista_entrega",
                table: "Solicitacoes",
                type: "smalldatetime",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "descricao",
                table: "Solicitacoes",
                type: "varchar(1000)",
                nullable: true);

            migrationBuilder.Sql("update tiposdespesa set reembolsar_colaborador =1, faturar_cliente =1, exibir_para_colaborador = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "exibir_para_colaborador",
                table: "TiposDespesa");

            migrationBuilder.DropColumn(
                name: "faturar_cliente",
                table: "TiposDespesa");

            migrationBuilder.DropColumn(
                name: "reembolsar_colaborador",
                table: "TiposDespesa");

            migrationBuilder.DropColumn(
                name: "data_entrega",
                table: "Solicitacoes");

            migrationBuilder.DropColumn(
                name: "data_prevista_entrega",
                table: "Solicitacoes");

            migrationBuilder.DropColumn(
                name: "descricao",
                table: "Solicitacoes");

            migrationBuilder.AlterColumn<string>(
                name: "objetivo",
                table: "Solicitacoes",
                type: "varchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(1000)");
        }
    }
}
