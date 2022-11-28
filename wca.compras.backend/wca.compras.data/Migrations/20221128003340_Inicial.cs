using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.compras.data.Migrations
{
    public partial class Inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Filiais",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filiais", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Perfil",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    descricao = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Perfil", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Permissao",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    regra = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    descricao = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissao", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TipoFornecimento",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoFornecimento", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    cnpj = table.Column<string>(type: "varchar(14)", maxLength: 14, nullable: false),
                    inscricao_estadual = table.Column<string>(type: "varchar(12)", maxLength: 12, nullable: true),
                    endereco = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true),
                    cep = table.Column<string>(type: "varchar(9)", maxLength: 9, nullable: true),
                    numero = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true),
                    cidade = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    uf = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: true),
                    ativo = table.Column<bool>(type: "bit", nullable: false),
                    filial_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.id);
                    table.ForeignKey(
                        name: "FK_Clientes_Filiais_filial_id",
                        column: x => x.filial_id,
                        principalTable: "Filiais",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PerfilPermissao",
                columns: table => new
                {
                    PerfilId = table.Column<int>(type: "int", nullable: false),
                    PermissaoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerfilPermissao", x => new { x.PerfilId, x.PermissaoId });
                    table.ForeignKey(
                        name: "FK_PerfilPermissao_Perfil_PerfilId",
                        column: x => x.PerfilId,
                        principalTable: "Perfil",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PerfilPermissao_Permissao_PermissaoId",
                        column: x => x.PermissaoId,
                        principalTable: "Permissao",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClienteContatos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    email = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true),
                    telefone = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    celular = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    aprova_pedido = table.Column<bool>(type: "bit", nullable: false),
                    cliente_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClienteContatos", x => x.id);
                    table.ForeignKey(
                        name: "FK_ClienteContatos_Clientes_cliente_id",
                        column: x => x.cliente_id,
                        principalTable: "Clientes",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ClienteOrcamentoConfiguracaos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cliente_id = table.Column<int>(type: "int", nullable: false),
                    tipofornecimento_id = table.Column<int>(type: "int", nullable: false),
                    valor_pedido = table.Column<decimal>(type: "money", nullable: false),
                    quantidade_mes = table.Column<int>(type: "int", nullable: false),
                    tolerancia = table.Column<int>(type: "int", nullable: false),
                    aprovador_por = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClienteOrcamentoConfiguracaos", x => x.id);
                    table.ForeignKey(
                        name: "FK_ClienteOrcamentoConfiguracaos_Clientes_cliente_id",
                        column: x => x.cliente_id,
                        principalTable: "Clientes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClienteOrcamentoConfiguracaos_TipoFornecimento_tipofornecimento_id",
                        column: x => x.tipofornecimento_id,
                        principalTable: "TipoFornecimento",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    password = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    ativo = table.Column<bool>(type: "bit", nullable: false),
                    cliente_id = table.Column<int>(type: "int", nullable: true),
                    perfil_id = table.Column<int>(type: "int", nullable: true),
                    filial_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Clientes_cliente_id",
                        column: x => x.cliente_id,
                        principalTable: "Clientes",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Usuarios_Filiais_filial_id",
                        column: x => x.filial_id,
                        principalTable: "Filiais",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Usuarios_Perfil_perfil_id",
                        column: x => x.perfil_id,
                        principalTable: "Perfil",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ResetPassword",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    token = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    data_criacao = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    data_expiracao = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    data_revogacao = table.Column<DateTime>(type: "smalldatetime", nullable: true),
                    usuario_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResetPassword", x => x.id);
                    table.ForeignKey(
                        name: "FK_ResetPassword_Usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "Usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClienteContatos_cliente_id",
                table: "ClienteContatos",
                column: "cliente_id");

            migrationBuilder.CreateIndex(
                name: "IX_ClienteOrcamentoConfiguracaos_cliente_id",
                table: "ClienteOrcamentoConfiguracaos",
                column: "cliente_id");

            migrationBuilder.CreateIndex(
                name: "IX_ClienteOrcamentoConfiguracaos_tipofornecimento_id",
                table: "ClienteOrcamentoConfiguracaos",
                column: "tipofornecimento_id");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_filial_id",
                table: "Clientes",
                column: "filial_id");

            migrationBuilder.CreateIndex(
                name: "IX_PerfilPermissao_PermissaoId",
                table: "PerfilPermissao",
                column: "PermissaoId");

            migrationBuilder.CreateIndex(
                name: "IX_ResetPassword_usuario_id",
                table: "ResetPassword",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_cliente_id",
                table: "Usuarios",
                column: "cliente_id");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_filial_id",
                table: "Usuarios",
                column: "filial_id");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_perfil_id",
                table: "Usuarios",
                column: "perfil_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClienteContatos");

            migrationBuilder.DropTable(
                name: "ClienteOrcamentoConfiguracaos");

            migrationBuilder.DropTable(
                name: "PerfilPermissao");

            migrationBuilder.DropTable(
                name: "ResetPassword");

            migrationBuilder.DropTable(
                name: "TipoFornecimento");

            migrationBuilder.DropTable(
                name: "Permissao");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Perfil");

            migrationBuilder.DropTable(
                name: "Filiais");
        }
    }
}
