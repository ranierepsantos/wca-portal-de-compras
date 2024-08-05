using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.share.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterTable_Vagas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vagas_StatusVaga_status_vaga_id",
                table: "Vagas");

            migrationBuilder.DropTable(
                name: "StatusVaga");

            migrationBuilder.RenameColumn(
                name: "status_vaga_id",
                table: "Vagas",
                newName: "status_solicitacao_id");

            migrationBuilder.RenameIndex(
                name: "IX_Vagas_status_vaga_id",
                table: "Vagas",
                newName: "IX_Vagas_status_solicitacao_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vagas_StatusSolicitacao_status_solicitacao_id",
                table: "Vagas",
                column: "status_solicitacao_id",
                principalTable: "StatusSolicitacao",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vagas_StatusSolicitacao_status_solicitacao_id",
                table: "Vagas");

            migrationBuilder.RenameColumn(
                name: "status_solicitacao_id",
                table: "Vagas",
                newName: "status_vaga_id");

            migrationBuilder.RenameIndex(
                name: "IX_Vagas_status_solicitacao_id",
                table: "Vagas",
                newName: "IX_Vagas_status_vaga_id");

            migrationBuilder.CreateTable(
                name: "StatusVaga",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    autorizar = table.Column<bool>(type: "bit", nullable: false),
                    color = table.Column<string>(type: "varchar(30)", nullable: false),
                    notifica = table.Column<int>(type: "int", nullable: false),
                    proximo_status_id = table.Column<int>(type: "int", nullable: true, comment: "Utilizado quando campo autorizar = 1, após aprovação mudará para o status indicado aqui"),
                    status = table.Column<string>(type: "varchar(150)", nullable: false),
                    status_intermediario = table.Column<string>(type: "varchar(150)", nullable: false),
                    template_notificacao = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusVaga", x => x.id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Vagas_StatusVaga_status_vaga_id",
                table: "Vagas",
                column: "status_vaga_id",
                principalTable: "StatusVaga",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
