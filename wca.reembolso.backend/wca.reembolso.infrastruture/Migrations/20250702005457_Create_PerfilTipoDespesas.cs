using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.reembolso.infrastruture.Migrations
{
    /// <inheritdoc />
    public partial class Create_PerfilTipoDespesas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Perfil_TipoDespesa",
                columns: table => new
                {
                    perfil_id = table.Column<int>(type: "int", nullable: false),
                    tipodespesa_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Perfil_TipoDespesa", x => new { x.perfil_id, x.tipodespesa_id });
                    table.ForeignKey(
                        name: "FK_Perfil_TipoDespesa_TiposDespesa_tipodespesa_id",
                        column: x => x.tipodespesa_id,
                        principalTable: "TiposDespesa",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Perfil_TipoDespesa_tipodespesa_id",
                table: "Perfil_TipoDespesa",
                column: "tipodespesa_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Perfil_TipoDespesa");
        }
    }
}
