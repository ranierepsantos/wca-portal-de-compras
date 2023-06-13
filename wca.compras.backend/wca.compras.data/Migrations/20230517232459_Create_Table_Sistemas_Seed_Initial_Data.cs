using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.compras.data.Migrations
{
    public partial class Create_Table_Sistemas_Seed_Initial_Data : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sistemas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    descricao = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sistemas", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "Sistemas",
                columns: new[] { "id", "descricao", "nome" },
                values: new object[] { 1, "Sistema de compras de insumos", "Compras" });

            migrationBuilder.InsertData(
                table: "Sistemas",
                columns: new[] { "id", "descricao", "nome" },
                values: new object[] { 2, "Sistema de solicitação de reembolso", "Reembolso" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sistemas");
        }
    }
}
