using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.reembolso.infrastruture.Migrations
{
    /// <inheritdoc />
    public partial class AlterTable_Faturamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Valor",
                table: "Faturamento",
                newName: "valor");

            migrationBuilder.AddColumn<string>(
                name: "documento_po",
                table: "Faturamento",
                type: "varchar(200)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "numero_po",
                table: "Faturamento",
                type: "varchar(20)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "documento_po",
                table: "Faturamento");

            migrationBuilder.DropColumn(
                name: "numero_po",
                table: "Faturamento");

            migrationBuilder.RenameColumn(
                name: "valor",
                table: "Faturamento",
                newName: "Valor");
        }
    }
}
