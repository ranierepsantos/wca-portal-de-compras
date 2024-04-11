using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.reembolso.infrastruture.Migrations
{
    /// <inheritdoc />
    public partial class AlterTable_Solicitacao_AddColumn_DataStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "data_status",
                table: "Solicitacoes",
                type: "smalldatetime",
                nullable: true);

            string sql = "update s set data_status = sh.data_hora from Solicitacoes s " +
                        "inner join SolicitacaoHistorico sh on sh.solicitacao_id = s.id " +
                        "where StatusSolicitacaoId =3 and s.tipo_solicitacao =2" +
                        "  and sh.evento like '%registrou pagamento%'";

            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "data_status",
                table: "Solicitacoes");
        }
    }
}
