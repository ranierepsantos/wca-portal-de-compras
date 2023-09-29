using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.reembolso.infrastruture.Migrations
{
    /// <inheritdoc />
    public partial class AlterTable_Faturamento_AddTable_StatusFaturamento_AddTable_FaturamentoHistorico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DataCriacao",
                table: "Faturamento",
                newName: "data_criacao");

            migrationBuilder.AlterColumn<string>(
                name: "documento_po",
                table: "Faturamento",
                type: "varchar(500)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "data_finalizacao",
                table: "Faturamento",
                type: "smalldatetime",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "notaFiscal",
                table: "Faturamento",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FaturamentoHistorico",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    faturamento_id = table.Column<int>(type: "int", nullable: false),
                    data_hora = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    evento = table.Column<string>(type: "varchar(500)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaturamentoHistorico", x => x.id);
                    table.ForeignKey(
                        name: "FK_FaturamentoHistorico_Faturamento_faturamento_id",
                        column: x => x.faturamento_id,
                        principalTable: "Faturamento",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FaturamentoHistorico_faturamento_id",
                table: "FaturamentoHistorico",
                column: "faturamento_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FaturamentoHistorico");

            migrationBuilder.DropColumn(
                name: "data_finalizacao",
                table: "Faturamento");

            migrationBuilder.DropColumn(
                name: "notaFiscal",
                table: "Faturamento");

            migrationBuilder.RenameColumn(
                name: "data_criacao",
                table: "Faturamento",
                newName: "DataCriacao");

            migrationBuilder.AlterColumn<string>(
                name: "documento_po",
                table: "Faturamento",
                type: "varchar(200)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldNullable: true);
        }
    }
}
