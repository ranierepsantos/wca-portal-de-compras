using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.compras.data.Migrations
{
    public partial class CreateTable_FornecedorContato : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FornecedorContatos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    email = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true),
                    telefone = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    celular = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    aprova_pedido = table.Column<bool>(type: "bit", nullable: false),
                    fornecedor_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FornecedorContatos", x => x.id);
                    table.ForeignKey(
                        name: "FK_FornecedorContatos_Fornecedores_fornecedor_id",
                        column: x => x.fornecedor_id,
                        principalTable: "Fornecedores",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FornecedorContatos_fornecedor_id",
                table: "FornecedorContatos",
                column: "fornecedor_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FornecedorContatos");
        }
    }
}
