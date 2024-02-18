using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.share.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class four : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solicitacoes_Usuarios_gestor_id",
                table: "Solicitacoes");

            migrationBuilder.AlterColumn<int>(
                name: "gestor_id",
                table: "Solicitacoes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitacoes_Usuarios_gestor_id",
                table: "Solicitacoes",
                column: "gestor_id",
                principalTable: "Usuarios",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solicitacoes_Usuarios_gestor_id",
                table: "Solicitacoes");

            migrationBuilder.AlterColumn<int>(
                name: "gestor_id",
                table: "Solicitacoes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitacoes_Usuarios_gestor_id",
                table: "Solicitacoes",
                column: "gestor_id",
                principalTable: "Usuarios",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
