using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.compras.data.Migrations
{
    public partial class Create_Configuracao_AlterTables_AddIcmsColumn_IPIColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "icms",
                table: "Requisicoes",
                type: "money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "valor_icms",
                table: "Requisicoes",
                type: "money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "valor_ipi",
                table: "RequisicaoItens",
                type: "money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "valor_ipi",
                table: "Produtos",
                type: "money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "icms",
                table: "Fornecedores",
                type: "money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Configuracoes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    chave = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    descricao = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    tipo_campo = table.Column<int>(type: "int", maxLength: 500, nullable: false),
                    valor = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    combo_valores = table.Column<string>(type: "varchar(8000)", maxLength: 8000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuracoes", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configuracoes");

            migrationBuilder.DropColumn(
                name: "icms",
                table: "Requisicoes");

            migrationBuilder.DropColumn(
                name: "valor_icms",
                table: "Requisicoes");

            migrationBuilder.DropColumn(
                name: "valor_ipi",
                table: "RequisicaoItens");

            migrationBuilder.DropColumn(
                name: "valor_ipi",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "icms",
                table: "Fornecedores");
            
        }
    }
}
