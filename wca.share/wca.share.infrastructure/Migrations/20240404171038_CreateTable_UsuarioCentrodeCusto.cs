using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.share.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateTable_UsuarioCentrodeCusto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsuarioCentrodeCustos",
                columns: table => new
                {
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    CentroCustoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioCentrodeCustos", x => new { x.UsuarioId, x.CentroCustoId });
                    table.ForeignKey(
                        name: "FK_UsuarioCentrodeCustos_CentrosDeCusto_CentroCustoId",
                        column: x => x.CentroCustoId,
                        principalTable: "CentrosDeCusto",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioCentrodeCustos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioCentrodeCustos_CentroCustoId",
                table: "UsuarioCentrodeCustos",
                column: "CentroCustoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsuarioCentrodeCustos");
        }
    }
}
