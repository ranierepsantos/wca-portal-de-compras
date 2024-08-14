using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.compras.data.Migrations
{
    public partial class AlterTable_Fornecedor_AddColumns_ValorFrete_ValorCompraMinimoSemFrete_TaxaGestaoMinimaPercentual : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "taxa_gestao_minima_percentual",
                table: "Fornecedores",
                type: "decimal(4,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "valor_compra_minimo_sem_frete",
                table: "Fornecedores",
                type: "money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "valor_frete",
                table: "Fornecedores",
                type: "money",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "taxa_gestao_minima_percentual",
                table: "Fornecedores");

            migrationBuilder.DropColumn(
                name: "valor_compra_minimo_sem_frete",
                table: "Fornecedores");

            migrationBuilder.DropColumn(
                name: "valor_frete",
                table: "Fornecedores");
        }
    }
}
