using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.compras.data.Migrations
{
    public partial class Create_Tables_Fornecedor_Produtos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fornecedores",
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
                    filial_id = table.Column<int>(type: "int", nullable: false),
                    tipofornecimento_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fornecedores", x => x.id);
                    table.ForeignKey(
                        name: "FK_Fornecedores_Filiais_filial_id",
                        column: x => x.filial_id,
                        principalTable: "Filiais",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Fornecedores_TipoFornecimento_tipofornecimento_id",
                        column: x => x.tipofornecimento_id,
                        principalTable: "TipoFornecimento",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    unidade_medida = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    valor = table.Column<decimal>(type: "money", nullable: false),
                    taxa_gestao = table.Column<decimal>(type: "money", nullable: false),
                    fornecedor_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.id);
                    table.ForeignKey(
                        name: "FK_Produtos_Fornecedores_fornecedor_id",
                        column: x => x.fornecedor_id,
                        principalTable: "Fornecedores",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fornecedores_filial_id",
                table: "Fornecedores",
                column: "filial_id");

            migrationBuilder.CreateIndex(
                name: "IX_Fornecedores_tipofornecimento_id",
                table: "Fornecedores",
                column: "tipofornecimento_id");

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_fornecedor_id",
                table: "Produtos",
                column: "fornecedor_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Produtos");

            migrationBuilder.DropTable(
                name: "Fornecedores");
        }
    }
}
