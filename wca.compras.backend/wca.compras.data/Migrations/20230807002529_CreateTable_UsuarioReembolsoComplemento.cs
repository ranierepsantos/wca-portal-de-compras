using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.compras.data.Migrations
{
    public partial class CreateTable_UsuarioReembolsoComplemento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsuarioReembolsoComplemento",
                columns: table => new
                {
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    gestor_id = table.Column<int>(type: "int", nullable: true),
                    cargo = table.Column<string>(type: "varchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioReembolsoComplemento", x => x.usuario_id);
                });
            migrationBuilder.CreateIndex(
                        name: "IX_UsuarioReembolsoComplemento_usuario_id",
                        table: "UsuarioReembolsoComplemento",
                        column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioReembolsoComplemento_gestor_id",
                table: "UsuarioReembolsoComplemento",
                column: "gestor_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsuarioReembolsoComplemento");
        }
    }
}
