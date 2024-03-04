using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.share.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class twelve : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.DropForeignKey(
                name: "FK_Solicitacoes_Usuarios_gestor_id",
                table: "Solicitacoes");

            migrationBuilder.Sql("update solicitacoes set gestor_id = null;");

            migrationBuilder.RenameColumn(
                name: "gestor_id",
                table: "Solicitacoes",
                newName: "centrocusto_id");

            migrationBuilder.RenameIndex(
                name: "IX_Solicitacoes_gestor_id",
                table: "Solicitacoes",
                newName: "IX_Solicitacoes_centrocusto_id");

            migrationBuilder.RenameColumn(
                name: "status_homologacao_sindicato",
                table: "SolicitacaoDesligamento",
                newName: "status_ficha_epi");

            migrationBuilder.CreateTable(
                name: "CentrosDeCusto",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    codigo = table.Column<int>(type: "int", nullable: false),
                    cliente_id = table.Column<int>(type: "int", nullable: false),
                    nome = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CentrosDeCusto", x => x.id);
                    table.ForeignKey(
                        name: "FK_CentrosDeCusto_Clientes_cliente_id",
                        column: x => x.cliente_id,
                        principalTable: "Clientes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CentrosDeCusto_cliente_id",
                table: "CentrosDeCusto",
                column: "cliente_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitacoes_CentrosDeCusto_centrocusto_id",
                table: "Solicitacoes",
                column: "centrocusto_id",
                principalTable: "CentrosDeCusto",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solicitacoes_CentrosDeCusto_centrocusto_id",
                table: "Solicitacoes");

            migrationBuilder.DropTable(
                name: "CentrosDeCusto");

            migrationBuilder.RenameColumn(
                name: "centrocusto_id",
                table: "Solicitacoes",
                newName: "gestor_id");

            migrationBuilder.RenameIndex(
                name: "IX_Solicitacoes_centrocusto_id",
                table: "Solicitacoes",
                newName: "IX_Solicitacoes_gestor_id");

            migrationBuilder.RenameColumn(
                name: "status_ficha_epi",
                table: "SolicitacaoDesligamento",
                newName: "status_homologacao_sindicato");

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitacoes_Usuarios_gestor_id",
                table: "Solicitacoes",
                column: "gestor_id",
                principalTable: "Usuarios",
                principalColumn: "id");
        }
    }
}
