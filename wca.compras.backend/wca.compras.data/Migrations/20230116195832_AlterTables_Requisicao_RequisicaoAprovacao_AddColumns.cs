using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.compras.data.Migrations
{
    public partial class AlterTables_Requisicao_RequisicaoAprovacao_AddColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "data_entrega",
                table: "Requisicoes",
                type: "smalldatetime",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "local_entrega",
                table: "Requisicoes",
                type: "varchar(300)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "requer_autorizacao_cliente",
                table: "Requisicoes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "requer_autorizacao_wca",
                table: "Requisicoes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "tipoAprovador",
                table: "RequisicaoAprovacoes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "data_entrega",
                table: "Requisicoes");

            migrationBuilder.DropColumn(
                name: "local_entrega",
                table: "Requisicoes");

            migrationBuilder.DropColumn(
                name: "requer_autorizacao_cliente",
                table: "Requisicoes");

            migrationBuilder.DropColumn(
                name: "requer_autorizacao_wca",
                table: "Requisicoes");

            migrationBuilder.DropColumn(
                name: "tipoAprovador",
                table: "RequisicaoAprovacoes");
        }
    }
}
