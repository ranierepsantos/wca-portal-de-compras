using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.share.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterTable_Funcionarios_AlterColumn_esocial_matricula : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Funcionarios_e-social-matricula",
                table: "Funcionarios");

            migrationBuilder.RenameColumn(
                name: "e-social-matricula",
                table: "Funcionarios",
                newName: "esocial_matricula");

            migrationBuilder.CreateIndex(
                name: "IX_Funcionarios_esocial_matricula",
                table: "Funcionarios",
                column: "esocial_matricula",
                unique: true,
                filter: "[esocial_matricula] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Funcionarios_esocial_matricula",
                table: "Funcionarios");

            migrationBuilder.RenameColumn(
                name: "esocial_matricula",
                table: "Funcionarios",
                newName: "e-social-matricula");

            migrationBuilder.CreateIndex(
                name: "IX_Funcionarios_e-social-matricula",
                table: "Funcionarios",
                column: "e-social-matricula",
                unique: true,
                filter: "[e-social-matricula] IS NOT NULL");
        }
    }
}
