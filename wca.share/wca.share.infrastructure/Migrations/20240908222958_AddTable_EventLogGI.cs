using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.share.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTable_EventLogGI : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventLogGi",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    data_hora = table.Column<string>(type: "varchar(50)", nullable: false),
                    entidade = table.Column<string>(type: "varchar(50)", nullable: false),
                    log = table.Column<string>(type: "varchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventLogGi", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventLogGi");
        }
    }
}
