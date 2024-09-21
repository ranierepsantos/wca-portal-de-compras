using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.reembolso.infrastruture.Migrations
{
    /// <inheritdoc />
    public partial class AlterTable_Solicitacao_AddColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "descricao",
                table: "Solicitacoes",
                type: "varchar(1000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(1000",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "marca",
                table: "Solicitacoes",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "quantidade",
                table: "Solicitacoes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "valor_frete",
                table: "Solicitacoes",
                type: "money",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "valor_unitario",
                table: "Solicitacoes",
                type: "money",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "marca",
                table: "Solicitacoes");

            migrationBuilder.DropColumn(
                name: "quantidade",
                table: "Solicitacoes");

            migrationBuilder.DropColumn(
                name: "valor_frete",
                table: "Solicitacoes");

            migrationBuilder.DropColumn(
                name: "valor_unitario",
                table: "Solicitacoes");

            migrationBuilder.AlterColumn<string>(
                name: "descricao",
                table: "Solicitacoes",
                type: "varchar(1000",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldNullable: true);
        }
    }
}
