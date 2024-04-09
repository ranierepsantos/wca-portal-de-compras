﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using wca.reembolso.infrastruture.Context;

#nullable disable

namespace wca.reembolso.infrastruture.Migrations
{
    [DbContext(typeof(WcaReembolsoContext))]
    partial class WcaReembolsoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("wca.reembolso.domain.Entities.CentroCusto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CentroCustoId")
                        .HasColumnType("int")
                        .HasColumnName("centrocusto_id");

                    b.Property<int>("ClienteId")
                        .HasColumnType("int")
                        .HasColumnName("cliente_id");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)")
                        .HasColumnName("nome");

                    b.HasKey("Id");

                    b.HasIndex("ClienteId", "CentroCustoId")
                        .IsUnique()
                        .HasDatabaseName("IDX_CentrosDeCustos_ClienteId_CentroCustoId_Unique");

                    b.ToTable("CentrosDeCustos");
                });

            modelBuilder.Entity("wca.reembolso.domain.Entities.Cliente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Ativo")
                        .HasColumnType("bit")
                        .HasColumnName("ativo");

                    b.Property<string>("CNPJ")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("cnpj");

                    b.Property<string>("Cep")
                        .HasMaxLength(9)
                        .HasColumnType("varchar(9)")
                        .HasColumnName("cep");

                    b.Property<string>("Cidade")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("cidade");

                    b.Property<string>("CodigoCliente")
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("codigo_cliente");

                    b.Property<string>("Endereco")
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)")
                        .HasColumnName("endereco");

                    b.Property<int>("FilialId")
                        .HasColumnType("int")
                        .HasColumnName("filial_id");

                    b.Property<string>("InscricaoEstadual")
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)")
                        .HasColumnName("inscricao_estadual");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)")
                        .HasColumnName("nome");

                    b.Property<string>("Numero")
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)")
                        .HasColumnName("numero");

                    b.Property<string>("UF")
                        .HasMaxLength(2)
                        .HasColumnType("varchar(2)")
                        .HasColumnName("uf");

                    b.Property<decimal>("ValorLimite")
                        .HasColumnType("money")
                        .HasColumnName("valor_limite");

                    b.HasKey("Id");

                    b.ToTable("Clientes");
                });

            modelBuilder.Entity("wca.reembolso.domain.Entities.ContaCorrente", b =>
                {
                    b.Property<int>("UsuarioId")
                        .HasColumnType("int")
                        .HasColumnName("usuario_id");

                    b.Property<decimal>("Saldo")
                        .HasColumnType("money")
                        .HasColumnName("saldo");

                    b.HasKey("UsuarioId");

                    b.ToTable("ContaCorrente");
                });

            modelBuilder.Entity("wca.reembolso.domain.Entities.Despesa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<byte?>("Aprovada")
                        .HasColumnType("tinyint")
                        .HasColumnName("aprovada");

                    b.Property<string>("CNPJ")
                        .IsRequired()
                        .HasColumnType("varchar(20)")
                        .HasColumnName("cnpj");

                    b.Property<DateTime?>("DataEvento")
                        .HasColumnType("smalldatetime")
                        .HasColumnName("data_evento");

                    b.Property<string>("Destino")
                        .IsRequired()
                        .HasColumnType("varchar(1000)")
                        .HasColumnName("destino");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("varchar(200)")
                        .HasColumnName("image_path");

                    b.Property<string>("InscricaoEstadual")
                        .IsRequired()
                        .HasColumnType("varchar(20)")
                        .HasColumnName("inscricao_estadual");

                    b.Property<decimal>("KmPercorrido")
                        .HasColumnType("decimal(10,3)")
                        .HasColumnName("km_percorrido");

                    b.Property<string>("Motivo")
                        .IsRequired()
                        .HasColumnType("varchar(1000)")
                        .HasColumnName("motivo");

                    b.Property<string>("NumeroFiscal")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasColumnName("numero_fiscal");

                    b.Property<string>("Observacao")
                        .HasColumnType("varchar(500)")
                        .HasColumnName("observacao");

                    b.Property<string>("Origem")
                        .IsRequired()
                        .HasColumnType("varchar(1000)")
                        .HasColumnName("origem");

                    b.Property<string>("RazaoSocial")
                        .IsRequired()
                        .HasColumnType("varchar(150)")
                        .HasColumnName("razao_social");

                    b.Property<int>("SolicitacaoId")
                        .HasColumnType("int")
                        .HasColumnName("solicitacao_id");

                    b.Property<int>("TipoDespesaId")
                        .HasColumnType("int")
                        .HasColumnName("tipodespesa_id");

                    b.Property<decimal>("Valor")
                        .HasColumnType("money")
                        .HasColumnName("valor");

                    b.HasKey("Id");

                    b.HasIndex("SolicitacaoId");

                    b.HasIndex("TipoDespesaId");

                    b.ToTable("Despesas");
                });

            modelBuilder.Entity("wca.reembolso.domain.Entities.Faturamento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ClienteId")
                        .HasColumnType("int")
                        .HasColumnName("cliente_id");

                    b.Property<DateTime>("DataCriacao")
                        .HasColumnType("smalldatetime")
                        .HasColumnName("data_criacao");

                    b.Property<DateTime?>("DataFinalizacao")
                        .HasColumnType("smalldatetime")
                        .HasColumnName("data_finalizacao");

                    b.Property<string>("DocumentoPO")
                        .HasColumnType("varchar(500)")
                        .HasColumnName("documento_po");

                    b.Property<string>("NotaFiscal")
                        .HasColumnType("varchar(20)")
                        .HasColumnName("notaFiscal");

                    b.Property<string>("NumeroPO")
                        .HasColumnType("varchar(20)")
                        .HasColumnName("numero_po");

                    b.Property<int>("Status")
                        .HasColumnType("int")
                        .HasColumnName("status");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int")
                        .HasColumnName("usuario_id");

                    b.Property<decimal>("Valor")
                        .HasColumnType("money")
                        .HasColumnName("valor");

                    b.HasKey("Id");

                    b.HasIndex("ClienteId");

                    b.ToTable("Faturamento");
                });

            modelBuilder.Entity("wca.reembolso.domain.Entities.FaturamentoHistorico", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DataHora")
                        .HasColumnType("smalldatetime")
                        .HasColumnName("data_hora");

                    b.Property<string>("Evento")
                        .IsRequired()
                        .HasColumnType("varchar(500)")
                        .HasColumnName("evento");

                    b.Property<int>("FaturamentoId")
                        .HasColumnType("int")
                        .HasColumnName("faturamento_id");

                    b.HasKey("Id");

                    b.HasIndex("FaturamentoId");

                    b.ToTable("FaturamentoHistorico");
                });

            modelBuilder.Entity("wca.reembolso.domain.Entities.FaturamentoItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("FaturamentoId")
                        .HasColumnType("int")
                        .HasColumnName("faturamento_id");

                    b.Property<int>("SolicitacaoId")
                        .HasColumnType("int")
                        .HasColumnName("solicitacao_id");

                    b.HasKey("Id");

                    b.HasIndex("FaturamentoId");

                    b.HasIndex("SolicitacaoId");

                    b.ToTable("FaturamentoItems");
                });

            modelBuilder.Entity("wca.reembolso.domain.Entities.Notificacao", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DataHora")
                        .HasColumnType("smalldatetime")
                        .HasColumnName("data_hora");

                    b.Property<string>("Entidade")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("entidade");

                    b.Property<int>("EntidadeId")
                        .HasColumnType("int")
                        .HasColumnName("entidade_id");

                    b.Property<bool>("Lido")
                        .HasColumnType("bit")
                        .HasColumnName("lido");

                    b.Property<string>("Nota")
                        .IsRequired()
                        .HasColumnType("varchar(500)")
                        .HasColumnName("nota");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int")
                        .HasColumnName("usuario_id");

                    b.HasKey("Id");

                    b.ToTable("Notificacoes");
                });

            modelBuilder.Entity("wca.reembolso.domain.Entities.Solicitacao", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CentroCustoId")
                        .HasColumnType("int");

                    b.Property<int>("ClienteId")
                        .HasColumnType("int")
                        .HasColumnName("cliente_id");

                    b.Property<string>("ColaboradorCargo")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasColumnName("colaborador_cargo");

                    b.Property<int>("ColaboradorId")
                        .HasColumnType("int")
                        .HasColumnName("colaborador_id");

                    b.Property<DateTime>("DataSolicitacao")
                        .HasColumnType("smalldatetime")
                        .HasColumnName("data_solicitacao");

                    b.Property<string>("Objetivo")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasColumnName("objetivo");

                    b.Property<DateTime?>("PeriodoFinal")
                        .HasColumnType("smalldatetime")
                        .HasColumnName("periodo_final");

                    b.Property<DateTime?>("PeriodoInicial")
                        .HasColumnType("smalldatetime")
                        .HasColumnName("periodo_inicial");

                    b.Property<string>("Projeto")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasColumnName("projeto");

                    b.Property<int>("Status")
                        .HasColumnType("int")
                        .HasColumnName("StatusSolicitacaoId");

                    b.Property<int>("StatusAnterior")
                        .HasColumnType("int")
                        .HasColumnName("StatusAnteriorId");

                    b.Property<int>("TipoSolicitacao")
                        .HasColumnType("int")
                        .HasColumnName("tipo_solicitacao");

                    b.Property<decimal>("ValorAdiantamento")
                        .HasColumnType("money")
                        .HasColumnName("valor_adiantamento");

                    b.Property<decimal>("ValorDespesa")
                        .HasColumnType("money")
                        .HasColumnName("valor_despesa");

                    b.HasKey("Id");

                    b.HasIndex("CentroCustoId");

                    b.HasIndex("ClienteId");

                    b.HasIndex("ColaboradorId");

                    b.ToTable("Solicitacoes");
                });

            modelBuilder.Entity("wca.reembolso.domain.Entities.SolicitacaoHistorico", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DataHora")
                        .HasColumnType("smalldatetime")
                        .HasColumnName("data_hora");

                    b.Property<string>("Evento")
                        .IsRequired()
                        .HasColumnType("varchar(500)")
                        .HasColumnName("evento");

                    b.Property<int>("SolicitacaoId")
                        .HasColumnType("int")
                        .HasColumnName("solicitacao_id");

                    b.HasKey("Id");

                    b.HasIndex("SolicitacaoId");

                    b.ToTable("SolicitacaoHistorico");
                });

            modelBuilder.Entity("wca.reembolso.domain.Entities.StatusSolicitacao", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Autorizar")
                        .HasColumnType("bit")
                        .HasColumnName("autorizar");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("varchar(30)")
                        .HasColumnName("color");

                    b.Property<int>("Notifica")
                        .HasColumnType("int")
                        .HasColumnName("notifica");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("varchar(150)")
                        .HasColumnName("texto");

                    b.Property<string>("TemplateNotificacao")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("template_notificacao");

                    b.HasKey("Id");

                    b.ToTable("StatusSolicitacao");
                });

            modelBuilder.Entity("wca.reembolso.domain.Entities.TipoDespesa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Ativo")
                        .HasColumnType("bit")
                        .HasColumnName("ativo");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("nome");

                    b.Property<int>("Tipo")
                        .HasColumnType("int")
                        .HasColumnName("tipo");

                    b.Property<decimal>("Valor")
                        .HasColumnType("money")
                        .HasColumnName("valor");

                    b.HasKey("Id");

                    b.ToTable("TiposDespesa");
                });

            modelBuilder.Entity("wca.reembolso.domain.Entities.Transacao", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ContaCorrenteUsuarioId")
                        .HasColumnType("int")
                        .HasColumnName("ContaCorrenteUsuarioId");

                    b.Property<DateTime>("DataHora")
                        .HasColumnType("smalldatetime")
                        .HasColumnName("data_hora");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("varchar(250)")
                        .HasColumnName("descricao");

                    b.Property<string>("Operador")
                        .IsRequired()
                        .HasColumnType("varchar(1)")
                        .HasColumnName("operador");

                    b.Property<decimal>("Saldo")
                        .HasColumnType("money")
                        .HasColumnName("saldo");

                    b.Property<decimal>("SaldoAnterior")
                        .HasColumnType("money")
                        .HasColumnName("saldo_anterior");

                    b.Property<decimal>("Valor")
                        .HasColumnType("money")
                        .HasColumnName("valor");

                    b.HasKey("Id");

                    b.HasIndex("ContaCorrenteUsuarioId");

                    b.ToTable("Transacoes");
                });

            modelBuilder.Entity("wca.reembolso.domain.Entities.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<bool>("Ativo")
                        .HasColumnType("bit")
                        .HasColumnName("ativo");

                    b.Property<string>("Celular")
                        .HasColumnType("varchar(30)")
                        .HasColumnName("celular");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)")
                        .HasColumnName("email");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("nome");

                    b.HasKey("Id");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("wca.reembolso.domain.Entities.UsuarioCentrodeCustos", b =>
                {
                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.Property<int>("CentroCustoId")
                        .HasColumnType("int");

                    b.HasKey("UsuarioId", "CentroCustoId");

                    b.HasIndex("CentroCustoId");

                    b.ToTable("UsuarioCentrodeCustos");
                });

            modelBuilder.Entity("wca.reembolso.domain.Entities.UsuarioClientes", b =>
                {
                    b.Property<int>("UsuarioId")
                        .HasColumnType("int")
                        .HasColumnName("usuario_id");

                    b.Property<int>("ClienteId")
                        .HasColumnType("int")
                        .HasColumnName("cliente_id");

                    b.HasKey("UsuarioId", "ClienteId");

                    b.HasIndex("ClienteId");

                    b.ToTable("UsuarioClientes");
                });

            modelBuilder.Entity("wca.reembolso.domain.Entities.UsuarioConfiguracoes", b =>
                {
                    b.Property<int>("UsuarioId")
                        .HasColumnType("int")
                        .HasColumnName("usuario_id");

                    b.Property<bool>("NotificarPorChatBot")
                        .HasColumnType("bit")
                        .HasColumnName("notificar_por_chatbot");

                    b.Property<bool>("NotificarPorEmail")
                        .HasColumnType("bit")
                        .HasColumnName("notificar_por_email");

                    b.HasKey("UsuarioId");

                    b.ToTable("UsuarioConfiguracoes");
                });

            modelBuilder.Entity("wca.reembolso.domain.Entities.CentroCusto", b =>
                {
                    b.HasOne("wca.reembolso.domain.Entities.Cliente", null)
                        .WithMany("CentroCusto")
                        .HasForeignKey("ClienteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("wca.reembolso.domain.Entities.Despesa", b =>
                {
                    b.HasOne("wca.reembolso.domain.Entities.Solicitacao", "Solicitacao")
                        .WithMany("Despesa")
                        .HasForeignKey("SolicitacaoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("wca.reembolso.domain.Entities.TipoDespesa", "TipoDespesa")
                        .WithMany()
                        .HasForeignKey("TipoDespesaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Solicitacao");

                    b.Navigation("TipoDespesa");
                });

            modelBuilder.Entity("wca.reembolso.domain.Entities.Faturamento", b =>
                {
                    b.HasOne("wca.reembolso.domain.Entities.Cliente", "Cliente")
                        .WithMany()
                        .HasForeignKey("ClienteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cliente");
                });

            modelBuilder.Entity("wca.reembolso.domain.Entities.FaturamentoHistorico", b =>
                {
                    b.HasOne("wca.reembolso.domain.Entities.Faturamento", null)
                        .WithMany("FaturamentoHistorico")
                        .HasForeignKey("FaturamentoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("wca.reembolso.domain.Entities.FaturamentoItem", b =>
                {
                    b.HasOne("wca.reembolso.domain.Entities.Faturamento", null)
                        .WithMany("FaturamentoItem")
                        .HasForeignKey("FaturamentoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("wca.reembolso.domain.Entities.Solicitacao", "Solicitacao")
                        .WithMany()
                        .HasForeignKey("SolicitacaoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Solicitacao");
                });

            modelBuilder.Entity("wca.reembolso.domain.Entities.Solicitacao", b =>
                {
                    b.HasOne("wca.reembolso.domain.Entities.CentroCusto", "CentroCusto")
                        .WithMany()
                        .HasForeignKey("CentroCustoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("wca.reembolso.domain.Entities.Cliente", "Cliente")
                        .WithMany("Solicitacoes")
                        .HasForeignKey("ClienteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("wca.reembolso.domain.Entities.Usuario", "Colaborador")
                        .WithMany()
                        .HasForeignKey("ColaboradorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CentroCusto");

                    b.Navigation("Cliente");

                    b.Navigation("Colaborador");
                });

            modelBuilder.Entity("wca.reembolso.domain.Entities.SolicitacaoHistorico", b =>
                {
                    b.HasOne("wca.reembolso.domain.Entities.Solicitacao", null)
                        .WithMany("SolicitacaoHistorico")
                        .HasForeignKey("SolicitacaoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("wca.reembolso.domain.Entities.Transacao", b =>
                {
                    b.HasOne("wca.reembolso.domain.Entities.ContaCorrente", "ContaCorrente")
                        .WithMany("Transacoes")
                        .HasForeignKey("ContaCorrenteUsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ContaCorrente");
                });

            modelBuilder.Entity("wca.reembolso.domain.Entities.Usuario", b =>
                {
                    b.HasOne("wca.reembolso.domain.Entities.ContaCorrente", "ContaCorrente")
                        .WithOne("Usuario")
                        .HasForeignKey("wca.reembolso.domain.Entities.Usuario", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ContaCorrente");
                });

            modelBuilder.Entity("wca.reembolso.domain.Entities.UsuarioCentrodeCustos", b =>
                {
                    b.HasOne("wca.reembolso.domain.Entities.CentroCusto", "CentroCusto")
                        .WithMany("Usuarios")
                        .HasForeignKey("CentroCustoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("wca.reembolso.domain.Entities.Usuario", "Usuario")
                        .WithMany("UsuarioCentrodeCustos")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CentroCusto");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("wca.reembolso.domain.Entities.UsuarioClientes", b =>
                {
                    b.HasOne("wca.reembolso.domain.Entities.Cliente", "Cliente")
                        .WithMany("UsuarioClientes")
                        .HasForeignKey("ClienteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("wca.reembolso.domain.Entities.Usuario", "Usuario")
                        .WithMany("UsuarioClientes")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cliente");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("wca.reembolso.domain.Entities.UsuarioConfiguracoes", b =>
                {
                    b.HasOne("wca.reembolso.domain.Entities.Usuario", "Usuario")
                        .WithOne("UsuarioConfiguracoes")
                        .HasForeignKey("wca.reembolso.domain.Entities.UsuarioConfiguracoes", "UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("wca.reembolso.domain.Entities.CentroCusto", b =>
                {
                    b.Navigation("Usuarios");
                });

            modelBuilder.Entity("wca.reembolso.domain.Entities.Cliente", b =>
                {
                    b.Navigation("CentroCusto");

                    b.Navigation("Solicitacoes");

                    b.Navigation("UsuarioClientes");
                });

            modelBuilder.Entity("wca.reembolso.domain.Entities.ContaCorrente", b =>
                {
                    b.Navigation("Transacoes");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("wca.reembolso.domain.Entities.Faturamento", b =>
                {
                    b.Navigation("FaturamentoHistorico");

                    b.Navigation("FaturamentoItem");
                });

            modelBuilder.Entity("wca.reembolso.domain.Entities.Solicitacao", b =>
                {
                    b.Navigation("Despesa");

                    b.Navigation("SolicitacaoHistorico");
                });

            modelBuilder.Entity("wca.reembolso.domain.Entities.Usuario", b =>
                {
                    b.Navigation("UsuarioCentrodeCustos");

                    b.Navigation("UsuarioClientes");

                    b.Navigation("UsuarioConfiguracoes");
                });
#pragma warning restore 612, 618
        }
    }
}
