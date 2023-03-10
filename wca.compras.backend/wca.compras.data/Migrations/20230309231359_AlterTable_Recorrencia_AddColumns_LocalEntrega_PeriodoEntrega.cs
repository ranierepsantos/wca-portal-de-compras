using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.compras.data.Migrations
{
    public partial class AlterTable_Recorrencia_AddColumns_LocalEntrega_PeriodoEntrega : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "local_entrega",
                table: "Recorrencias",
                type: "varchar(300)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "periodo_entrega",
                table: "Recorrencias",
                type: "varchar(1500)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "local_entrega",
                table: "Recorrencias");

            migrationBuilder.DropColumn(
                name: "periodo_entrega",
                table: "Recorrencias");
        }
    }
}
