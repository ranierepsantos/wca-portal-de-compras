using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.reembolso.infrastruture.Migrations
{
    /// <inheritdoc />
    public partial class Initial_CreateTable_Clientes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "clientes",
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
                    ativo = table.Column<bool>(type: "bit", nullable: false),
                    valor_limite = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clientes", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "clientes");
        }
    }
}
