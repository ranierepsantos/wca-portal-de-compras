using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.compras.data.Migrations
{
    public partial class AlterTable_Requisicao_DropColumnIcms_AddColumns_Endereco : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "icms",
                table: "Requisicoes");

            migrationBuilder.DropColumn(
                name: "local_entrega",
                table: "Requisicoes");

            migrationBuilder.AddColumn<string>(
                name: "cep",
                table: "Requisicoes",
                type: "varchar(9)",
                maxLength: 9,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "cidade",
                table: "Requisicoes",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "endereco",
                table: "Requisicoes",
                type: "varchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "numero",
                table: "Requisicoes",
                type: "varchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "uf",
                table: "Requisicoes",
                type: "varchar(2)",
                maxLength: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "icms",
                table: "RequisicaoItens",
                type: "money",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cep",
                table: "Requisicoes");

            migrationBuilder.DropColumn(
                name: "cidade",
                table: "Requisicoes");

            migrationBuilder.DropColumn(
                name: "endereco",
                table: "Requisicoes");

            migrationBuilder.DropColumn(
                name: "numero",
                table: "Requisicoes");

            migrationBuilder.DropColumn(
                name: "uf",
                table: "Requisicoes");

            migrationBuilder.DropColumn(
                name: "icms",
                table: "RequisicaoItens");

            migrationBuilder.AddColumn<decimal>(
                name: "icms",
                table: "Requisicoes",
                type: "money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "local_entrega",
                table: "Requisicoes",
                type: "varchar(300)",
                nullable: true);
        }
    }
}
