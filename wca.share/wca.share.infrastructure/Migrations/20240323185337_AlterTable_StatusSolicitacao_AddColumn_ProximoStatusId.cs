using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.share.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterTable_StatusSolicitacao_AddColumn_ProximoStatusId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "proximo_status_id",
                table: "StatusSolicitacao",
                type: "int",
                nullable: true,
                comment: "Utilizado quando campo autorizar = 1, após aprovação mudará para o status indicado aqui");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "proximo_status_id",
                table: "StatusSolicitacao");
        }
    }
}
