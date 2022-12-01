using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.compras.data.Migrations
{
    public partial class AlterFilialAddColumnAtivo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ativo",
                table: "Filiais",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ativo",
                table: "Filiais");
        }
    }
}
