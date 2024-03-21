using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.share.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterTable_SolicitacaoDesligamento_AddColumns_StatusBeneficio_StatusReembolso : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "status_beneficio",
                table: "SolicitacaoDesligamento",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "status_reembolso",
                table: "SolicitacaoDesligamento",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status_beneficio",
                table: "SolicitacaoDesligamento");

            migrationBuilder.DropColumn(
                name: "status_reembolso",
                table: "SolicitacaoDesligamento");
        }
    }
}
