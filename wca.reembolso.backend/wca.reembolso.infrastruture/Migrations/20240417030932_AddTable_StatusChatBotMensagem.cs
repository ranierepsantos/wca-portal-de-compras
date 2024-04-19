using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.reembolso.infrastruture.Migrations
{
    /// <inheritdoc />
    public partial class AddTable_StatusChatBotMensagem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Status_ChatBot_Mensagem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    status_solicitacao_id = table.Column<int>(type: "int", nullable: false),
                    tipo_solicitacao = table.Column<int>(type: "int", nullable: false, comment: "Tipo de solicitação que será enviado: 0 - Ambas 1 - Reembolso, 2 - Adiantamento"),
                    enviar_para = table.Column<int>(type: "int", nullable: false, comment: "Tipo de usuário que será enviado: 1- Gestor WCA, 2 - Gestor Cliente, 3 - Colaborador"),
                    mensagem = table.Column<string>(type: "varchar(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status_ChatBot_Mensagem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Status_ChatBot_Mensagem_StatusSolicitacao_status_solicitacao_id",
                        column: x => x.status_solicitacao_id,
                        principalTable: "StatusSolicitacao",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Status_ChatBot_Mensagem_status_solicitacao_id_enviar_para",
                table: "Status_ChatBot_Mensagem",
                columns: new[] { "status_solicitacao_id", "enviar_para" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Status_ChatBot_Mensagem");
        }
    }
}
