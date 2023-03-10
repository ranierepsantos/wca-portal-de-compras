using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.compras.data.Migrations
{
    public partial class Configuracao_AddData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Configuracoes",
                columns: new[] { "id", "chave", "combo_valores", "descricao", "tipo_campo", "valor" },
                values: new object[] { 1, "requisicao.sendemail.fornecedor", "", "Requisição solicitar aprovação fornecedor", 1, "false" });

            migrationBuilder.InsertData(
                table: "Configuracoes",
                columns: new[] { "id", "chave", "combo_valores", "descricao", "tipo_campo", "valor" },
                values: new object[] { 2, "requisicao.datacorte", "[{\"value\":1,\"text\":\"01\"},{\"value\":2,\"text\":\"02\"},{\"value\":3,\"text\":\"03\"},{\"value\":4,\"text\":\"04\"},{\"value\":5,\"text\":\"05\"},{\"value\":6,\"text\":\"06\"},{\"value\":7,\"text\":\"07\"},{\"value\":8,\"text\":\"08\"},{\"value\":9,\"text\":\"09\"},{\"value\":10,\"text\":\"10\"},{\"value\":11,\"text\":\"11\"},{\"value\":12,\"text\":\"12\"},{\"value\":13,\"text\":\"13\"},{\"value\":14,\"text\":\"14\"},{\"value\":15,\"text\":\"15\"},{\"value\":16,\"text\":\"16\"},{\"value\":17,\"text\":\"17\"},{\"value\":18,\"text\":\"18\"},{\"value\":19,\"text\":\"19\"},{\"value\":20,\"text\":\"20\"},{\"value\":21,\"text\":\"21\"},{\"value\":22,\"text\":\"22\"},{\"value\":23,\"text\":\"23\"},{\"value\":24,\"text\":\"24\"},{\"value\":25,\"text\":\"25\"},{\"value\":26,\"text\":\"26\"},{\"value\":27,\"text\":\"27\"},{\"value\":28,\"text\":\"28\"},{\"value\":29,\"text\":\"29\"},{\"value\":30,\"text\":\"30\"},{\"value\":31,\"text\":\"31\"}]", "Requisição Data de Corte", 5, "1" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Configuracoes",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Configuracoes",
                keyColumn: "id",
                keyValue: 2);
        }
    }
}
