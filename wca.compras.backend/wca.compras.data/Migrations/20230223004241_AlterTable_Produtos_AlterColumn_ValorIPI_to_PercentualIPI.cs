using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.compras.data.Migrations
{
    public partial class AlterTable_Produtos_AlterColumn_ValorIPI_to_PercentualIPI : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "valor_ipi",
                table: "Produtos");

            migrationBuilder.AddColumn<decimal>(
                name: "percentual_ipi",
                table: "Produtos",
                type: "decimal(3,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "percentual_ipi",
                table: "Produtos");

            migrationBuilder.AddColumn<decimal>(
                name: "valor_ipi",
                table: "Produtos",
                type: "money",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
