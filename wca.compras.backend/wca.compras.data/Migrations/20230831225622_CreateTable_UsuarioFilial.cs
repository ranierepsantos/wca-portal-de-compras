using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.compras.data.Migrations
{
    public partial class CreateTable_UsuarioFilial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioReembolsoComplemento_Usuarios_gestor_id",
                table: "UsuarioReembolsoComplemento");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Filiais_filial_id",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_filial_id",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "filial_id",
                table: "Usuarios");

            migrationBuilder.AlterColumn<int>(
                name: "gestor_id",
                table: "UsuarioReembolsoComplemento",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "FilialUsuario",
                columns: table => new
                {
                    FilialId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilialUsuario", x => new { x.FilialId, x.UsuarioId });
                    table.ForeignKey(
                        name: "FK_FilialUsuario_Filiais_FilialId",
                        column: x => x.FilialId,
                        principalTable: "Filiais",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilialUsuario_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FilialUsuario_UsuarioId",
                table: "FilialUsuario",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioReembolsoComplemento_Usuarios_gestor_id",
                table: "UsuarioReembolsoComplemento",
                column: "gestor_id",
                principalTable: "Usuarios",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioReembolsoComplemento_Usuarios_gestor_id",
                table: "UsuarioReembolsoComplemento");

            migrationBuilder.DropTable(
                name: "FilialUsuario");

            migrationBuilder.AddColumn<int>(
                name: "filial_id",
                table: "Usuarios",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "gestor_id",
                table: "UsuarioReembolsoComplemento",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_filial_id",
                table: "Usuarios",
                column: "filial_id");

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioReembolsoComplemento_Usuarios_gestor_id",
                table: "UsuarioReembolsoComplemento",
                column: "gestor_id",
                principalTable: "Usuarios",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Filiais_filial_id",
                table: "Usuarios",
                column: "filial_id",
                principalTable: "Filiais",
                principalColumn: "id");
        }
    }
}
