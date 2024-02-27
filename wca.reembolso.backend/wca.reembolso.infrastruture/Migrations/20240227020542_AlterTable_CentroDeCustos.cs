using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.reembolso.infrastruture.Migrations
{
    /// <inheritdoc />
    public partial class AlterTable_CentroDeCustos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE Solicitacoes set gestor_id = null");

            migrationBuilder.DropForeignKey(
                name: "FK_Solicitacoes_Usuarios_gestor_id",
                table: "Solicitacoes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CentrosDeCustos",
                table: "CentrosDeCustos");

            migrationBuilder.DropIndex(
                name: "IX_CentrosDeCustos_cliente_id",
                table: "CentrosDeCustos");

            migrationBuilder.RenameColumn(
                name: "gestor_id",
                table: "Solicitacoes",
                newName: "CentroCustoId");

            migrationBuilder.RenameIndex(
                name: "IX_Solicitacoes_gestor_id",
                table: "Solicitacoes",
                newName: "IX_Solicitacoes_CentroCustoId");

            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "CentrosDeCustos",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CentrosDeCustos",
                table: "CentrosDeCustos",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IDX_CentrosDeCustos_ClienteId_CentroCustoId_Unique",
                table: "CentrosDeCustos",
                columns: new[] { "cliente_id", "centrocusto_id" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitacoes_CentrosDeCustos_CentroCustoId",
                table: "Solicitacoes",
                column: "CentroCustoId",
                principalTable: "CentrosDeCustos",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solicitacoes_CentrosDeCustos_CentroCustoId",
                table: "Solicitacoes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CentrosDeCustos",
                table: "CentrosDeCustos");

            migrationBuilder.DropIndex(
                name: "IDX_CentrosDeCustos_ClienteId_CentroCustoId_Unique",
                table: "CentrosDeCustos");

            migrationBuilder.DropColumn(
                name: "id",
                table: "CentrosDeCustos");

            migrationBuilder.RenameColumn(
                name: "CentroCustoId",
                table: "Solicitacoes",
                newName: "gestor_id");

            migrationBuilder.RenameIndex(
                name: "IX_Solicitacoes_CentroCustoId",
                table: "Solicitacoes",
                newName: "IX_Solicitacoes_gestor_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CentrosDeCustos",
                table: "CentrosDeCustos",
                columns: new[] { "centrocusto_id", "cliente_id" });

            migrationBuilder.CreateIndex(
                name: "IX_CentrosDeCustos_cliente_id",
                table: "CentrosDeCustos",
                column: "cliente_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitacoes_Usuarios_gestor_id",
                table: "Solicitacoes",
                column: "gestor_id",
                principalTable: "Usuarios",
                principalColumn: "id");
        }
    }
}
