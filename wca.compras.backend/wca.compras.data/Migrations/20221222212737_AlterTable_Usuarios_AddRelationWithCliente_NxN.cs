using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.compras.data.Migrations
{
    public partial class AlterTable_Usuarios_AddRelationWithCliente_NxN : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Clientes_cliente_id",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_cliente_id",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "cliente_id",
                table: "Usuarios");

            migrationBuilder.CreateTable(
                name: "ClienteUsuario",
                columns: table => new
                {
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClienteUsuario", x => new { x.ClienteId, x.UsuarioId });
                    table.ForeignKey(
                        name: "FK_ClienteUsuario_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClienteUsuario_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClienteUsuario_UsuarioId",
                table: "ClienteUsuario",
                column: "UsuarioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClienteUsuario");

            migrationBuilder.AddColumn<int>(
                name: "cliente_id",
                table: "Usuarios",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_cliente_id",
                table: "Usuarios",
                column: "cliente_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Clientes_cliente_id",
                table: "Usuarios",
                column: "cliente_id",
                principalTable: "Clientes",
                principalColumn: "id");
        }
    }
}
