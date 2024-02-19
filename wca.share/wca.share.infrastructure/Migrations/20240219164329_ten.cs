using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.share.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ten : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "cliente_id",
                table: "Funcionarios",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "gestor_id",
                table: "Funcionarios",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Funcionarios_cliente_id",
                table: "Funcionarios",
                column: "cliente_id");

            migrationBuilder.CreateIndex(
                name: "IX_Funcionarios_gestor_id",
                table: "Funcionarios",
                column: "gestor_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Funcionarios_Clientes_cliente_id",
                table: "Funcionarios",
                column: "cliente_id",
                principalTable: "Clientes",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Funcionarios_Usuarios_gestor_id",
                table: "Funcionarios",
                column: "gestor_id",
                principalTable: "Usuarios",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Funcionarios_Clientes_cliente_id",
                table: "Funcionarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Funcionarios_Usuarios_gestor_id",
                table: "Funcionarios");

            migrationBuilder.DropIndex(
                name: "IX_Funcionarios_cliente_id",
                table: "Funcionarios");

            migrationBuilder.DropIndex(
                name: "IX_Funcionarios_gestor_id",
                table: "Funcionarios");

            migrationBuilder.DropColumn(
                name: "cliente_id",
                table: "Funcionarios");

            migrationBuilder.DropColumn(
                name: "gestor_id",
                table: "Funcionarios");
        }
    }
}
