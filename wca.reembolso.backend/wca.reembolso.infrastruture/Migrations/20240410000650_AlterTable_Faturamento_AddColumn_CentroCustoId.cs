using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.reembolso.infrastruture.Migrations
{
    /// <inheritdoc />
    public partial class AlterTable_Faturamento_AddColumn_CentroCustoId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "centrocusto_id",
                table: "Faturamento",
                type: "int",
                nullable: true,
                defaultValue: null);

            migrationBuilder.CreateIndex(
                name: "IX_Faturamento_centrocusto_id",
                table: "Faturamento",
                column: "centrocusto_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Faturamento_CentrosDeCustos_centrocusto_id",
                table: "Faturamento",
                column: "centrocusto_id",
                principalTable: "CentrosDeCustos",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Faturamento_CentrosDeCustos_centrocusto_id",
                table: "Faturamento");

            migrationBuilder.DropIndex(
                name: "IX_Faturamento_centrocusto_id",
                table: "Faturamento");

            migrationBuilder.DropColumn(
                name: "centrocusto_id",
                table: "Faturamento");
        }
    }
}
