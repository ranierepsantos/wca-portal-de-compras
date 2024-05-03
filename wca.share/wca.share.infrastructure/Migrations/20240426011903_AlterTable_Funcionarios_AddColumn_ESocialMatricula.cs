using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.share.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterTable_Funcionarios_AddColumn_ESocialMatricula : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "numero_pis",
                table: "Funcionarios");

            migrationBuilder.AddColumn<string>(
                name: "e-social-matricula",
                table: "Funcionarios",
                type: "varchar(30)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Funcionarios_e-social-matricula",
                table: "Funcionarios",
                column: "e-social-matricula",
                unique: true,
                filter: "[e-social-matricula] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Funcionarios_e-social-matricula",
                table: "Funcionarios");

            migrationBuilder.DropColumn(
                name: "e-social-matricula",
                table: "Funcionarios");

            migrationBuilder.AddColumn<string>(
                name: "numero_pis",
                table: "Funcionarios",
                type: "varchar(20)",
                nullable: true);
        }
    }
}
