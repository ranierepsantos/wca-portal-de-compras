using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.compras.data.Migrations
{
    public partial class Alter_Table_Permissao_Add_Relation_With_Sistema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "sistema_id",
                table: "Permissao",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permissao_sistema_id",
                table: "Permissao",
                column: "sistema_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Permissao_Sistemas_sistema_id",
                table: "Permissao",
                column: "sistema_id",
                principalTable: "Sistemas",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permissao_Sistemas_sistema_id",
                table: "Permissao");

            migrationBuilder.DropIndex(
                name: "IX_Permissao_sistema_id",
                table: "Permissao");

            migrationBuilder.DropColumn(
                name: "sistema_id",
                table: "Permissao");
        }
    }
}
