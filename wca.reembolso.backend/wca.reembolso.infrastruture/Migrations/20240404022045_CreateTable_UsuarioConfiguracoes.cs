using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.reembolso.infrastruture.Migrations
{
    /// <inheritdoc />
    public partial class CreateTable_UsuarioConfiguracoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsuarioConfiguracoes",
                columns: table => new
                {
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    notificar_por_email = table.Column<bool>(type: "bit", nullable: false),
                    notificar_por_chatbot = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioConfiguracoes", x => x.usuario_id);
                    table.ForeignKey(
                        name: "FK_UsuarioConfiguracoes_Usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "Usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsuarioConfiguracoes");
        }
    }
}
