using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.share.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterTable_Solicitacao_AddColumn_CriadoPor_Funcionario_AddIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "criado_por",
                table: "Solicitacoes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Funcionarios_nome",
                table: "Funcionarios",
                column: "nome");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Funcionarios_nome",
                table: "Funcionarios");

            migrationBuilder.DropColumn(
                name: "criado_por",
                table: "Solicitacoes");
        }
    }
}
