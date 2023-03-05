using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.compras.data.Migrations
{
    public partial class Seed_Data_Permissao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "valor_ipi",
                table: "RequisicaoItens");

            migrationBuilder.AddColumn<decimal>(
                name: "percentual_ipi",
                table: "RequisicaoItens",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.InsertData(
                table: "Permissao",
                columns: new[] { "id", "descricao", "nome", "regra" },
                values: new object[] { 11, "Permite alterar configurações do sistema", "Configurações", "configuracao" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permissao",
                keyColumn: "id",
                keyValue: 11);

            migrationBuilder.DropColumn(
                name: "percentual_ipi",
                table: "RequisicaoItens");

            migrationBuilder.AddColumn<decimal>(
                name: "valor_ipi",
                table: "RequisicaoItens",
                type: "money",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
