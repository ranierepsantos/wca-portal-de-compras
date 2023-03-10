using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.compras.data.Migrations
{
    public partial class AddTable_TipoFornecimentoUsuario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TipoFornecimentoUsuario",
                columns: table => new
                {
                    TipoFornecimentoId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoFornecimentoUsuario", x => new { x.TipoFornecimentoId, x.UsuarioId });
                    table.ForeignKey(
                        name: "FK_TipoFornecimentoUsuario_TipoFornecimento_TipoFornecimentoId",
                        column: x => x.TipoFornecimentoId,
                        principalTable: "TipoFornecimento",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TipoFornecimentoUsuario_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TipoFornecimentoUsuario_UsuarioId",
                table: "TipoFornecimentoUsuario",
                column: "UsuarioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TipoFornecimentoUsuario");
        }
    }
}
