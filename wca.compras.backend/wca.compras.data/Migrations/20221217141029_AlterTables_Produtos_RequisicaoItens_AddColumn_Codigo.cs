using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.compras.data.Migrations
{
    public partial class AlterTables_Produtos_RequisicaoItens_AddColumn_Codigo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "produto_nome",
                table: "RequisicaoItens",
                newName: "nome");

            migrationBuilder.AddColumn<string>(
                name: "codigo",
                table: "RequisicaoItens",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "codigo",
                table: "Produtos",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "codigo",
                table: "RequisicaoItens");

            migrationBuilder.DropColumn(
                name: "codigo",
                table: "Produtos");

            migrationBuilder.RenameColumn(
                name: "nome",
                table: "RequisicaoItens",
                newName: "produto_nome");
        }
    }
}
