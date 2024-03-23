using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.share.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTable_Configuracoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Configuracoes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    chave = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    descricao = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    tipo_campo = table.Column<int>(type: "int", maxLength: 500, nullable: false),
                    valor = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    combo_valores = table.Column<string>(type: "varchar(8000)", maxLength: 8000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuracoes", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configuracoes");
        }
    }
}
