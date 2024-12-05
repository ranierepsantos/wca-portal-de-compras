using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace wca.share.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class seed_database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ItensMudanca",
                columns: new[] { "id", "ativo", "descricao" },
                values: new object[,]
                {
                    { 1, true, "Alteração de benefícios" },
                    { 2, true, "Alteração de salário" },
                    { 3, true, "Alteração de horário" },
                    { 4, true, "Exclusão insalubridade" },
                    { 5, true, "Exclusão periculosidade" },
                    { 6, true, "Inclusão insalubridade" },
                    { 7, true, "Inclusão periculosidade" }
                });

            migrationBuilder.InsertData(
                table: "StatusSolicitacao",
                columns: new[] { "id", "autorizar", "color", "notifica", "proximo_status_id", "status", "status_intermediario", "template_notificacao" },
                values: new object[,]
                {
                    { 1, false, "warning", 1, null, "Pendente", "Pendente", "Novo(a) {TipoSolicitacao} solicitado(a) - código: #{id}!" },
                    { 2, false, "info", 0, null, "Em Andamento", "Em Andamento", null },
                    { 3, false, "success", 0, null, "Concluído", "Concluído", null },
                    { 4, true, "gray", 1, 1, "Pendente", "Aguardando Aprovação", "{TipoSolicitacao} #{id} está aguardando aprovação!" },
                    { 5, false, "red", 1, null, "Concluído", "Reprovado", "{TipoSolicitacao} #{id} reprovado(a)!" },
                    { 6, false, "red", 0, null, "Concluído", "Cancelado", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ItensMudanca",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ItensMudanca",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ItensMudanca",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ItensMudanca",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ItensMudanca",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ItensMudanca",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ItensMudanca",
                keyColumn: "id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "StatusSolicitacao",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "StatusSolicitacao",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "StatusSolicitacao",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "StatusSolicitacao",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "StatusSolicitacao",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "StatusSolicitacao",
                keyColumn: "id",
                keyValue: 6);
        }
    }
}
