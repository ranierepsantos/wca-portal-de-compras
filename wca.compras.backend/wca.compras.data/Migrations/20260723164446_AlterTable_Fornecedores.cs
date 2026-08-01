using Microsoft.EntityFrameworkCore.Migrations;
using System.Text.RegularExpressions;

#nullable disable

namespace wca.compras.data.Migrations
{
    public partial class AlterTable_Fornecedores : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "permite_alterar_valorproduto",
                table: "Fornecedores",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.Sql("INSERT INTO Permissao(nome, regra, descricao, sistema_id) VALUES('Requisição - Alterar valores produto', 'requisicao-alterar-valor-produto', 'Permitir alterar valores do produto', 1)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "permite_alterar_valorproduto",
                table: "Fornecedores");

            //Excluir a permissão
            migrationBuilder.Sql("DELETE FROM Permissao WHERE regra = 'requisicao-alterar-valor-produto' AND sistema_id = 1");
        }
    }
}
