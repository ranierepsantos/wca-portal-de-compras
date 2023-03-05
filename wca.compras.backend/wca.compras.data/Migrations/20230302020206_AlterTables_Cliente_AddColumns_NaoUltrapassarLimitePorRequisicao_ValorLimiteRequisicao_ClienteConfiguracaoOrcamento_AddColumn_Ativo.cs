using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.compras.data.Migrations
{
    public partial class AlterTables_Cliente_AddColumns_NaoUltrapassarLimitePorRequisicao_ValorLimiteRequisicao_ClienteConfiguracaoOrcamento_AddColumn_Ativo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "nao_ultrapassar_limite_por_requisicao",
                table: "Clientes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "valor_limite_requisicao",
                table: "Clientes",
                type: "money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "ativo",
                table: "ClienteOrcamentoConfiguracaos",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "nao_ultrapassar_limite_por_requisicao",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "valor_limite_requisicao",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "ativo",
                table: "ClienteOrcamentoConfiguracaos");
        }
    }
}
