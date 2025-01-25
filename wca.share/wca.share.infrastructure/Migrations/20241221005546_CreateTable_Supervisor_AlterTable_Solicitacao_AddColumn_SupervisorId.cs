using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.share.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateTable_Supervisor_AlterTable_Solicitacao_AddColumn_SupervisorId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "supervisor_id",
                table: "Solicitacoes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Supervisor",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "varchar(200)", nullable: false),
                    ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supervisor", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacoes_supervisor_id",
                table: "Solicitacoes",
                column: "supervisor_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitacoes_Supervisor_supervisor_id",
                table: "Solicitacoes",
                column: "supervisor_id",
                principalTable: "Supervisor",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solicitacoes_Supervisor_supervisor_id",
                table: "Solicitacoes");

            migrationBuilder.DropTable(
                name: "Supervisor");

            migrationBuilder.DropIndex(
                name: "IX_Solicitacoes_supervisor_id",
                table: "Solicitacoes");

            migrationBuilder.DropColumn(
                name: "supervisor_id",
                table: "Solicitacoes");
        }
    }
}
