using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.compras.data.Migrations
{
    public partial class AlterTable_Filial_AddColumn_SistemaId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requisicoes_Filiais_filial_id",
                table: "Requisicoes");

            migrationBuilder.AlterColumn<int>(
                name: "filial_id",
                table: "Requisicoes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "sistema_id",
                table: "Filiais",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Requisicoes_Filiais_filial_id",
                table: "Requisicoes",
                column: "filial_id",
                principalTable: "Filiais",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requisicoes_Filiais_filial_id",
                table: "Requisicoes");

            migrationBuilder.DropColumn(
                name: "sistema_id",
                table: "Filiais");

            migrationBuilder.AlterColumn<int>(
                name: "filial_id",
                table: "Requisicoes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Requisicoes_Filiais_filial_id",
                table: "Requisicoes",
                column: "filial_id",
                principalTable: "Filiais",
                principalColumn: "id");
        }
    }
}
