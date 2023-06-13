using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.compras.data.Migrations
{
    public partial class AlterTable_Sistema_AddColumn_Icon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "icon",
                table: "Sistemas",
                type: "varchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Sistemas",
                keyColumn: "id",
                keyValue: 1,
                column: "icon",
                value: "");

            migrationBuilder.UpdateData(
                table: "Sistemas",
                keyColumn: "id",
                keyValue: 2,
                column: "icon",
                value: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "icon",
                table: "Sistemas");
        }
    }
}
