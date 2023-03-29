using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.compras.data.Migrations
{
    public partial class AlterTable_Recorrencia_DropColumn_LocalEntrega_AddColumns_Endereco : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "local_entrega",
                table: "Recorrencias");

            migrationBuilder.AddColumn<string>(
                name: "cep",
                table: "Recorrencias",
                type: "varchar(9)",
                maxLength: 9,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "cidade",
                table: "Recorrencias",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "endereco",
                table: "Recorrencias",
                type: "varchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "numero",
                table: "Recorrencias",
                type: "varchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "uf",
                table: "Recorrencias",
                type: "varchar(2)",
                maxLength: 2,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cep",
                table: "Recorrencias");

            migrationBuilder.DropColumn(
                name: "cidade",
                table: "Recorrencias");

            migrationBuilder.DropColumn(
                name: "endereco",
                table: "Recorrencias");

            migrationBuilder.DropColumn(
                name: "numero",
                table: "Recorrencias");

            migrationBuilder.DropColumn(
                name: "uf",
                table: "Recorrencias");

            migrationBuilder.AddColumn<string>(
                name: "local_entrega",
                table: "Recorrencias",
                type: "varchar(300)",
                nullable: false,
                defaultValue: "");
        }
    }
}
