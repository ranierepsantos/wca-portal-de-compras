using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.reembolso.infrastruture.Migrations
{
    /// <inheritdoc />
    public partial class AlterDatabase_CreateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Faturamento_clientes_cliente_id",
                table: "Faturamento");

            migrationBuilder.DropForeignKey(
                name: "FK_Solicitacoes_StatusSolicitacao_StatusSolicitacaoId",
                table: "Solicitacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Solicitacoes_clientes_cliente_id",
                table: "Solicitacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioClientes_clientes_cliente_id",
                table: "UsuarioClientes");

            migrationBuilder.DropIndex(
                name: "IX_Solicitacoes_StatusSolicitacaoId",
                table: "Solicitacoes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_clientes",
                table: "clientes");

            migrationBuilder.RenameTable(
                name: "clientes",
                newName: "Clientes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Clientes",
                table: "Clientes",
                column: "id");

            migrationBuilder.CreateTable(
                name: "Notificacoes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    data_hora = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    nota = table.Column<string>(type: "varchar(500)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notificacoes", x => x.id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Faturamento_Clientes_cliente_id",
                table: "Faturamento",
                column: "cliente_id",
                principalTable: "Clientes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitacoes_Clientes_cliente_id",
                table: "Solicitacoes",
                column: "cliente_id",
                principalTable: "Clientes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioClientes_Clientes_cliente_id",
                table: "UsuarioClientes",
                column: "cliente_id",
                principalTable: "Clientes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Faturamento_Clientes_cliente_id",
                table: "Faturamento");

            migrationBuilder.DropForeignKey(
                name: "FK_Solicitacoes_Clientes_cliente_id",
                table: "Solicitacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_UsuarioClientes_Clientes_cliente_id",
                table: "UsuarioClientes");

            migrationBuilder.DropTable(
                name: "Notificacoes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Clientes",
                table: "Clientes");

            migrationBuilder.RenameTable(
                name: "Clientes",
                newName: "clientes");

            migrationBuilder.AddColumn<int>(
                name: "status_id",
                table: "Solicitacoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_clientes",
                table: "clientes",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacoes_StatusSolicitacaoId",
                table: "Solicitacoes",
                column: "StatusSolicitacaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Faturamento_clientes_cliente_id",
                table: "Faturamento",
                column: "cliente_id",
                principalTable: "clientes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitacoes_StatusSolicitacao_StatusSolicitacaoId",
                table: "Solicitacoes",
                column: "StatusSolicitacaoId",
                principalTable: "StatusSolicitacao",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitacoes_clientes_cliente_id",
                table: "Solicitacoes",
                column: "cliente_id",
                principalTable: "clientes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsuarioClientes_clientes_cliente_id",
                table: "UsuarioClientes",
                column: "cliente_id",
                principalTable: "clientes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
