using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.compras.data.Migrations
{
    public partial class Create_Table_UsuarioSistemaPerfil : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuario_Sistema_Perfil",
                columns: table => new
                {
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    sistema_id = table.Column<int>(type: "int", nullable: false),
                    perfil_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario_Sistema_Perfil", x => new { x.usuario_id, x.sistema_id, x.perfil_id });
                    table.ForeignKey(
                        name: "FK_Usuario_Sistema_Perfil_Perfil_perfil_id",
                        column: x => x.perfil_id,
                        principalTable: "Perfil",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Usuario_Sistema_Perfil_Sistemas_sistema_id",
                        column: x => x.sistema_id,
                        principalTable: "Sistemas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Usuario_Sistema_Perfil_Usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "Usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Sistema_Perfil_perfil_id",
                table: "Usuario_Sistema_Perfil",
                column: "perfil_id");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Sistema_Perfil_sistema_id",
                table: "Usuario_Sistema_Perfil",
                column: "sistema_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Usuario_Sistema_Perfil");
        }
    }
}
