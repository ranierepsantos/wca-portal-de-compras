using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.compras.data.Migrations
{
    public partial class AddData_Permissao_Categoria : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissao",
                columns: new[] { "id", "descricao", "nome", "regra" },
                values: new object[] { 13, "Permite incluir e alterar categorias", "Categorias", "categoria" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permissao",
                keyColumn: "id",
                keyValue: 13);
        }
    }
}
