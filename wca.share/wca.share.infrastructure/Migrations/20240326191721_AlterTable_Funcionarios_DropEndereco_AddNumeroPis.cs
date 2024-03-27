using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.share.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterTable_Funcionarios_DropEndereco_AddNumeroPis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "bairro",
                table: "Funcionarios");

            migrationBuilder.DropColumn(
                name: "cep",
                table: "Funcionarios");

            migrationBuilder.DropColumn(
                name: "cidade",
                table: "Funcionarios");

            migrationBuilder.DropColumn(
                name: "complemento",
                table: "Funcionarios");

            migrationBuilder.DropColumn(
                name: "endereco",
                table: "Funcionarios");

            migrationBuilder.DropColumn(
                name: "uf",
                table: "Funcionarios");

            migrationBuilder.AddColumn<string>(
                name: "numero_pis",
                table: "Funcionarios",
                type: "varchar(20)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "numero_pis",
                table: "Funcionarios");

            migrationBuilder.AddColumn<string>(
                name: "bairro",
                table: "Funcionarios",
                type: "varchar(150)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "cep",
                table: "Funcionarios",
                type: "varchar(10)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "cidade",
                table: "Funcionarios",
                type: "varchar(150)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "complemento",
                table: "Funcionarios",
                type: "varchar(200)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "endereco",
                table: "Funcionarios",
                type: "varchar(200)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "uf",
                table: "Funcionarios",
                type: "varchar(2)",
                nullable: true);
        }
    }
}
