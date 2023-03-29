using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.compras.data.Migrations
{
    public partial class AlterTable_Fornecedor_DropColumn_Icms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "icms",
                table: "Fornecedores");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "icms",
                table: "Fornecedores",
                type: "money",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
