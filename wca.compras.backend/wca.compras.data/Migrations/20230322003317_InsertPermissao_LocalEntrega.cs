using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.compras.data.Migrations
{
    public partial class InsertPermissao_LocalEntrega : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissao",
                columns: new[] { "id", "descricao", "nome", "regra" },
                values: new object[] { 12, "Permite alterar o local de entrega da requisição", "Requisição - Local de entrega", "requisicao_local_entrega" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permissao",
                keyColumn: "id",
                keyValue: 12);
        }
    }
}
