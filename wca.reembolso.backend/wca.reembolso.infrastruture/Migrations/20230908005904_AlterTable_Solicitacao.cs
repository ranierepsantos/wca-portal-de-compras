using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.reembolso.infrastruture.Migrations
{
    /// <inheritdoc />
    public partial class AlterTable_Solicitacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "gestor_id",
                table: "Solicitacoes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacoes_colaborador_id",
                table: "Solicitacoes",
                column: "colaborador_id");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacoes_gestor_id",
                table: "Solicitacoes",
                column: "gestor_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitacoes_Usuarios_colaborador_id",
                table: "Solicitacoes",
                column: "colaborador_id",
                principalTable: "Usuarios",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_Solicitacoes_Usuarios_colaborador_id",
                table: "Solicitacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Solicitacoes_Usuarios_gestor_id",
                table: "Solicitacoes");

            migrationBuilder.DropIndex(
                name: "IX_Solicitacoes_colaborador_id",
                table: "Solicitacoes");

            migrationBuilder.DropIndex(
                name: "IX_Solicitacoes_gestor_id",
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
        }
    }
}
