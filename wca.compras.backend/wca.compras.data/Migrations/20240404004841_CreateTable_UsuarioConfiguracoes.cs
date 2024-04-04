using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.compras.data.Migrations
{
    public partial class CreateTable_UsuarioConfiguracoes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsuarioConfiguracoes",
                columns: table => new
                {
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    sistema_id = table.Column<int>(type: "int", nullable: false),
                    notificar_por_email = table.Column<bool>(type: "bit", nullable: false),
                    notificar_por_chatbot = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioConfiguracoes", x => new { x.usuario_id, x.sistema_id });
                    table.ForeignKey(
                        name: "FK_UsuarioConfiguracoes_Sistemas_sistema_id",
                        column: x => x.sistema_id,
                        principalTable: "Sistemas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioConfiguracoes_Usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "Usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioConfiguracoes_sistema_id",
                table: "UsuarioConfiguracoes",
                column: "sistema_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsuarioConfiguracoes");
        }
    }
}
