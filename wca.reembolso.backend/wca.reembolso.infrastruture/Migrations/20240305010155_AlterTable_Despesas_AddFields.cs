using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.reembolso.infrastruture.Migrations
{
    /// <inheritdoc />
    public partial class AlterTable_Despesas_AddFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "aprovada",
                table: "Despesas",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "conferida",
                table: "Despesas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "observacao",
                table: "Despesas",
                type: "varchar(500)",
                nullable: true);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "aprovada",
                table: "Despesas");

            migrationBuilder.DropColumn(
                name: "conferida",
                table: "Despesas");

            migrationBuilder.DropColumn(
                name: "observacao",
                table: "Despesas");
        }
    }
}
