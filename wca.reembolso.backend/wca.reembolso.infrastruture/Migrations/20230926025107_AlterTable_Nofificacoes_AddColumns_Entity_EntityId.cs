using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.reembolso.infrastruture.Migrations
{
    /// <inheritdoc />
    public partial class AlterTable_Nofificacoes_AddColumns_Entity_EntityId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notificacoes_Usuarios_usuario_id",
                table: "Notificacoes");

            migrationBuilder.DropIndex(
                name: "IX_Notificacoes_usuario_id",
                table: "Notificacoes");

            migrationBuilder.AddColumn<string>(
                name: "entidade",
                table: "Notificacoes",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "entidade_id",
                table: "Notificacoes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "entidade",
                table: "Notificacoes");

            migrationBuilder.DropColumn(
                name: "entidade_id",
                table: "Notificacoes");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacoes_usuario_id",
                table: "Notificacoes",
                column: "usuario_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notificacoes_Usuarios_usuario_id",
                table: "Notificacoes",
                column: "usuario_id",
                principalTable: "Usuarios",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
