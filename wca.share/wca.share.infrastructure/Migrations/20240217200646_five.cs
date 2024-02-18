using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.share.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class five : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusSolicitacaoId",
                table: "Solicitacoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SolicitacaoHistorico",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    solicitacao_id = table.Column<int>(type: "int", nullable: false),
                    data_hora = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    evento = table.Column<string>(type: "varchar(500)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitacaoHistorico", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "StatusSolicitacao",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    status = table.Column<string>(type: "varchar(150)", nullable: false),
                    status_intermediario = table.Column<string>(type: "varchar(150)", nullable: false),
                    color = table.Column<string>(type: "varchar(30)", nullable: false),
                    notifica = table.Column<int>(type: "int", nullable: false),
                    autorizar = table.Column<bool>(type: "bit", nullable: false),
                    template_notificacao = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusSolicitacao", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SolicitacaoHistorico");

            migrationBuilder.DropTable(
                name: "StatusSolicitacao");

            migrationBuilder.DropColumn(
                name: "StatusSolicitacaoId",
                table: "Solicitacoes");
        }
    }
}
