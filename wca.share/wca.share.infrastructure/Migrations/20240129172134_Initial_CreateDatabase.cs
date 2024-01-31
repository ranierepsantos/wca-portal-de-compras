using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.share.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial_CreateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    filial_id = table.Column<int>(type: "int", nullable: false),
                    nome = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    cnpj = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    inscricao_estadual = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    endereco = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true),
                    cep = table.Column<string>(type: "varchar(9)", maxLength: 9, nullable: true),
                    numero = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true),
                    cidade = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    uf = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: true),
                    ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Notificacoes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    data_hora = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    nota = table.Column<string>(type: "varchar(500)", nullable: false),
                    entidade = table.Column<string>(type: "varchar(50)", nullable: false),
                    entidade_id = table.Column<int>(type: "int", nullable: false),
                    lido = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notificacoes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    nome = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioClientes",
                columns: table => new
                {
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    cliente_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioClientes", x => new { x.usuario_id, x.cliente_id });
                    table.ForeignKey(
                        name: "FK_UsuarioClientes_Clientes_cliente_id",
                        column: x => x.cliente_id,
                        principalTable: "Clientes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioClientes_Usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "Usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioClientes_cliente_id",
                table: "UsuarioClientes",
                column: "cliente_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notificacoes");

            migrationBuilder.DropTable(
                name: "UsuarioClientes");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
