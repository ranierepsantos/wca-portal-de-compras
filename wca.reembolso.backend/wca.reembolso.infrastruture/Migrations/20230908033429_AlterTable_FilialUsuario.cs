using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.reembolso.infrastruture.Migrations
{
    /// <inheritdoc />
    public partial class AlterTable_FilialUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FilialUsuario",
                table: "FilialUsuario");

            migrationBuilder.DropIndex(
                name: "IX_FilialUsuario_UsuarioId",
                table: "FilialUsuario");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FilialUsuario",
                table: "FilialUsuario",
                columns: new[] { "UsuarioId", "FilialId" });

            migrationBuilder.CreateIndex(
                name: "IX_FilialUsuario_FilialId",
                table: "FilialUsuario",
                column: "FilialId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FilialUsuario",
                table: "FilialUsuario");

            migrationBuilder.DropIndex(
                name: "IX_FilialUsuario_FilialId",
                table: "FilialUsuario");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FilialUsuario",
                table: "FilialUsuario",
                columns: new[] { "FilialId", "UsuarioId" });

            migrationBuilder.CreateIndex(
                name: "IX_FilialUsuario_UsuarioId",
                table: "FilialUsuario",
                column: "UsuarioId");
        }
    }
}
