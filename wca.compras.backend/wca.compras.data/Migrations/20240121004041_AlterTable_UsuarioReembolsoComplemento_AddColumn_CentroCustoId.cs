using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.compras.data.Migrations
{
    public partial class AlterTable_UsuarioReembolsoComplemento_AddColumn_CentroCustoId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "centrocusto_id",
                table: "UsuarioReembolsoComplemento",
                type: "int",
                nullable: true,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "centrocusto_id",
                table: "UsuarioReembolsoComplemento"
            );
        }
    }
}
