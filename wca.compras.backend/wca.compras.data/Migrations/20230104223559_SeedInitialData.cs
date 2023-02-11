using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.compras.data.Migrations
{
    public partial class SeedInitialData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Perfil",
                columns: new[] { "id", "ativo", "descricao", "nome" },
                values: new object[] { 1, true, "Administrador do sistema", "Administrador" });

            migrationBuilder.InsertData(
                table: "Permissao",
                columns: new[] { "id", "descricao", "nome", "regra" },
                values: new object[,]
                {
                    { 1, "Permite alterar, incluir e visualizar dados da filial", "Filial", "filial" },
                    { 2, "Permite alterar, incluir e visualizar dados do cliente", "Clientes", "cliente" },
                    { 3, "Permite alterar, incluir e visualizar dados do usuário", "Usuários", "usuario" },
                    { 4, "Permite alterar, incluir e visualizar dados do perfil", "Perfil", "perfil" },
                    { 5, "Permite alterar, incluir e visualizar dados do fornecedor", "Fornecedores", "fornecedor" },
                    { 6, "Permite alterar, incluir e visualizar dados da requisição do usuário", "Requisição", "requisicao" },
                    { 7, "Permite visualizar dados de requisição de todos usuários", "Administrador Requisição", "requisicao_all_users" },
                    { 8, "Permite aprovar requisição", "Aprovar Requisição", "aprova_requisicao" }
                });
            migrationBuilder.InsertData(
                table: "PerfilPermissao",
                columns: new[] { "PerfilId", "PermissaoId"},
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 1, 3 },
                    { 1, 4 },
                    { 1, 5 },
                    { 1, 6 },
                    { 1, 7 },
                    { 1, 8 }
                });

            migrationBuilder.InsertData(
                table: "Filiais",
                columns: new[] {"id", "nome", "ativo"},
                values: new object[] {1, "Matriz", true});

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new [] { "id", "nome", "email", "password", "ativo", "perfil_id", "filial_id" },
                values: new object[,]
                {
                    { 1, "Adm NorgeLabs", "adm@norgelabs.com", "$2a$11$n54mszMqlS1q4ACb6DcwpeI.cZKqWHRmQtC4Rh4Jl9e6QtIZ3.9r.", true, 1, 1 }
                }
            );

            migrationBuilder.InsertData(
                table: "TipoFornecimento",
                columns: new[] { "id", "nome", "ativo" },
                values: new object[,] {
                    {1, "Descartáveis", true },
                    {2, "Higiene Pessoal", true },
                    {3, "Insumos de Limpeza", true }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Perfil",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Permissao",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Permissao",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Permissao",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Permissao",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Permissao",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Permissao",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Permissao",
                keyColumn: "id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Permissao",
                keyColumn: "id",
                keyValue: 8);
        }
    }
}
