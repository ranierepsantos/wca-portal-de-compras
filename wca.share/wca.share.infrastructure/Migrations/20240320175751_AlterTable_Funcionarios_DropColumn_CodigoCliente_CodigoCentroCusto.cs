using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.share.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterTable_Funcionarios_DropColumn_CodigoCliente_CodigoCentroCusto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "codigo_centrocusto",
                table: "Funcionarios");

            migrationBuilder.DropColumn(
                name: "codigo_cliente",
                table: "Funcionarios");

            migrationBuilder.AlterColumn<int>(
                name: "codigo_funcionario",
                table: "Funcionarios",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "codigo_funcionario",
                table: "Funcionarios",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "codigo_centrocusto",
                table: "Funcionarios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "codigo_cliente",
                table: "Funcionarios",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
