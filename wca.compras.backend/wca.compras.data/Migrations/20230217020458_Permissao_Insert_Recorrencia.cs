using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.compras.data.Migrations
{
    public partial class Permissao_Insert_Recorrencia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissao",
                columns: new[] { "id", "descricao", "nome", "regra" },
                values: new object[] { 9, "Permite incluir, alterar e inativar recorrência", "Recorrência", "recorrencia" });

            migrationBuilder.InsertData(
                table: "Permissao",
                columns: new[] { "id", "descricao", "nome", "regra" },
                values: new object[] { 10, "Permite alterar e inativar recorrência de outros usuários", "Administrador Recorrência", "recorrencias_view_others_users" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permissao",
                keyColumn: "id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Permissao",
                keyColumn: "id",
                keyValue: 10);
        }
    }
}
