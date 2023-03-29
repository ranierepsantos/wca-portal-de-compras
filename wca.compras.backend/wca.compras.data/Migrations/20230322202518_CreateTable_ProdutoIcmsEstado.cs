using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.compras.data.Migrations
{
    public partial class CreateTable_ProdutoIcmsEstado : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Produto_Icms_Estado",
                columns: table => new
                {
                    produto_id = table.Column<int>(type: "int", nullable: false),
                    uf = table.Column<string>(type: "varchar(2)", nullable: false),
                    icms = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produto_Icms_Estado", x => new { x.produto_id, x.uf });
                    table.ForeignKey(
                        name: "FK_Produto_Icms_Estado_Produtos_produto_id",
                        column: x => x.produto_id,
                        principalTable: "Produtos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Configuracoes",
                columns: new[] { "id", "chave", "combo_valores", "descricao", "tipo_campo", "valor" },
                values: new object[] { 3, "requisicao.sendemail.cliente", "", "Requisição - solicitar aprovação do cliente", 1, "false" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Produto_Icms_Estado");

            migrationBuilder.DeleteData(
                table: "Configuracoes",
                keyColumn: "id",
                keyValue: 3);
        }
    }
}
