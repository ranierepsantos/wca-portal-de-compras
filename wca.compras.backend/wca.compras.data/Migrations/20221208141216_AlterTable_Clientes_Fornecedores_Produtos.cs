using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.compras.data.Migrations
{
    public partial class AlterTable_Clientes_Fornecedores_Produtos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fornecedores_TipoFornecimento_tipofornecimento_id",
                table: "Fornecedores");

            migrationBuilder.DropIndex(
                name: "IX_Fornecedores_tipofornecimento_id",
                table: "Fornecedores");

            migrationBuilder.DropColumn(
                name: "tipofornecimento_id",
                table: "Fornecedores");

            migrationBuilder.AddColumn<int>(
                name: "tipofornecimento_id",
                table: "Produtos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "inscricao_estadual",
                table: "Fornecedores",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(12)",
                oldMaxLength: 12,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "cnpj",
                table: "Fornecedores",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(14)",
                oldMaxLength: 14);

            migrationBuilder.AlterColumn<string>(
                name: "inscricao_estadual",
                table: "Clientes",
                type: "varchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(12)",
                oldMaxLength: 12,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "cnpj",
                table: "Clientes",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(14)",
                oldMaxLength: 14);

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_tipofornecimento_id",
                table: "Produtos",
                column: "tipofornecimento_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Produtos_TipoFornecimento_tipofornecimento_id",
                table: "Produtos",
                column: "tipofornecimento_id",
                principalTable: "TipoFornecimento",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produtos_TipoFornecimento_tipofornecimento_id",
                table: "Produtos");

            migrationBuilder.DropIndex(
                name: "IX_Produtos_tipofornecimento_id",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "tipofornecimento_id",
                table: "Produtos");

            migrationBuilder.AlterColumn<string>(
                name: "inscricao_estadual",
                table: "Fornecedores",
                type: "varchar(12)",
                maxLength: 12,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "cnpj",
                table: "Fornecedores",
                type: "varchar(14)",
                maxLength: 14,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<int>(
                name: "tipofornecimento_id",
                table: "Fornecedores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "inscricao_estadual",
                table: "Clientes",
                type: "varchar(12)",
                maxLength: 12,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "cnpj",
                table: "Clientes",
                type: "varchar(14)",
                maxLength: 14,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldMaxLength: 20);

            migrationBuilder.CreateIndex(
                name: "IX_Fornecedores_tipofornecimento_id",
                table: "Fornecedores",
                column: "tipofornecimento_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Fornecedores_TipoFornecimento_tipofornecimento_id",
                table: "Fornecedores",
                column: "tipofornecimento_id",
                principalTable: "TipoFornecimento",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
