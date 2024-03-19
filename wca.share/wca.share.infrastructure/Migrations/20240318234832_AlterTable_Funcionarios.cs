using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wca.share.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterTable_Funcionarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Funcionarios_Clientes_cliente_id",
                table: "Funcionarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Funcionarios_Usuarios_gestor_id",
                table: "Funcionarios");

            migrationBuilder.DropIndex(
                name: "IX_Funcionarios_gestor_id",
                table: "Funcionarios");

            migrationBuilder.DropColumn(
                name: "codigo_gi",
                table: "Funcionarios");

            migrationBuilder.RenameColumn(
                name: "gestor_id",
                table: "Funcionarios",
                newName: "numero_celular");

            migrationBuilder.AlterColumn<int>(
                name: "cliente_id",
                table: "Funcionarios",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "bairro",
                table: "Funcionarios",
                type: "varchar(150)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "centrocusto_id",
                table: "Funcionarios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "cep",
                table: "Funcionarios",
                type: "varchar(10)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "cidade",
                table: "Funcionarios",
                type: "varchar(150)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "codigo_centrocusto",
                table: "Funcionarios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "codigo_cliente",
                table: "Funcionarios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "codigo_funcionario",
                table: "Funcionarios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "complemento",
                table: "Funcionarios",
                type: "varchar(200)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "data_admissao",
                table: "Funcionarios",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "data_demissao",
                table: "Funcionarios",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ddd_celular",
                table: "Funcionarios",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "Funcionarios",
                type: "varchar(200)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "endereco",
                table: "Funcionarios",
                type: "varchar(200)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "uf",
                table: "Funcionarios",
                type: "varchar(2)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Funcionarios_centrocusto_id",
                table: "Funcionarios",
                column: "centrocusto_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Funcionarios_CentrosDeCusto_centrocusto_id",
                table: "Funcionarios",
                column: "centrocusto_id",
                principalTable: "CentrosDeCusto",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Funcionarios_Clientes_cliente_id",
                table: "Funcionarios",
                column: "cliente_id",
                principalTable: "Clientes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Funcionarios_CentrosDeCusto_centrocusto_id",
                table: "Funcionarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Funcionarios_Clientes_cliente_id",
                table: "Funcionarios");

            migrationBuilder.DropIndex(
                name: "IX_Funcionarios_centrocusto_id",
                table: "Funcionarios");

            migrationBuilder.DropColumn(
                name: "bairro",
                table: "Funcionarios");

            migrationBuilder.DropColumn(
                name: "centrocusto_id",
                table: "Funcionarios");

            migrationBuilder.DropColumn(
                name: "cep",
                table: "Funcionarios");

            migrationBuilder.DropColumn(
                name: "cidade",
                table: "Funcionarios");

            migrationBuilder.DropColumn(
                name: "codigo_centrocusto",
                table: "Funcionarios");

            migrationBuilder.DropColumn(
                name: "codigo_cliente",
                table: "Funcionarios");

            migrationBuilder.DropColumn(
                name: "codigo_funcionario",
                table: "Funcionarios");

            migrationBuilder.DropColumn(
                name: "complemento",
                table: "Funcionarios");

            migrationBuilder.DropColumn(
                name: "data_admissao",
                table: "Funcionarios");

            migrationBuilder.DropColumn(
                name: "data_demissao",
                table: "Funcionarios");

            migrationBuilder.DropColumn(
                name: "ddd_celular",
                table: "Funcionarios");

            migrationBuilder.DropColumn(
                name: "email",
                table: "Funcionarios");

            migrationBuilder.DropColumn(
                name: "endereco",
                table: "Funcionarios");

            migrationBuilder.DropColumn(
                name: "uf",
                table: "Funcionarios");

            migrationBuilder.RenameColumn(
                name: "numero_celular",
                table: "Funcionarios",
                newName: "gestor_id");

            migrationBuilder.AlterColumn<int>(
                name: "cliente_id",
                table: "Funcionarios",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "codigo_gi",
                table: "Funcionarios",
                type: "varchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Funcionarios_gestor_id",
                table: "Funcionarios",
                column: "gestor_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Funcionarios_Clientes_cliente_id",
                table: "Funcionarios",
                column: "cliente_id",
                principalTable: "Clientes",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Funcionarios_Usuarios_gestor_id",
                table: "Funcionarios",
                column: "gestor_id",
                principalTable: "Usuarios",
                principalColumn: "id");
        }
    }
}
