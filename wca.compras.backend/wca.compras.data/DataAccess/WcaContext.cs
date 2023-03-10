using Microsoft.EntityFrameworkCore;
using wca.compras.domain.Entities;

namespace wca.compras.data.DataAccess
{
    public class WcaContext : DbContext
    {
        public WcaContext(DbContextOptions options) : base(options) {}

        public DbSet<Filial> Filiais { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Perfil> Perfis { get; set; }
        public DbSet<Permissao> Permissoes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<ResetPassword> ResetPassword { get; set; }
        public DbSet<ClienteContato> ClienteContatos { get; set; }
        public DbSet<ClienteOrcamentoConfiguracao> ClienteOrcamentoConfiguracaos { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Requisicao> Requisicoes { get; set; }
        public DbSet<RequisicaoItem> RequisicaoItens { get; set; }
        public DbSet<RequisicaoHistorico> RequisicaoHistoricos { get; set; }
        public DbSet<FornecedorContato> FornecedorContatos { get; set; }
        public DbSet<RequisicaoAprovacao> RequisicaoAprovacoes { get; set; }
        public DbSet<Recorrencia> Recorrencias { get; set; }
        public DbSet<RecorrenciaProduto> RecorrenciaProdutos { get; set; }
        public DbSet<RecorrenciaLog> RecorrenciaLogs { get; set; }
        public DbSet<Configuracao> Configuracoes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Permissao>().HasData(
                new Permissao()
                {
                    Id = 9,
                    Nome = "Recorrência",
                    Descricao = "Permite incluir, alterar e inativar recorrência",
                    Regra = "recorrencia",
                },
                new Permissao()
                {
                    Id = 10,
                    Nome = "Administrador Recorrência",
                    Descricao = "Permite alterar e inativar recorrência de outros usuários",
                    Regra = "recorrencias_view_others_users",
                }
            );

            modelBuilder.Entity<Configuracao>().HasData(
                new Configuracao() { 
                    Id = 1,
                    Chave = "requisicao.sendemail.fornecedor",
                    Descricao = "Requisição solicitar aprovação fornecedor",
                    Valor = "false",
                    TipoCampo = TipoCampoConfiguracao.Bool,
                    ComboValores =""
                },
                new Configuracao()
                {
                    Id = 2,
                    Chave = "requisicao.datacorte",
                    Descricao = "Requisição Data de Corte",
                    Valor = "1",
                    TipoCampo = TipoCampoConfiguracao.Combo,
                    ComboValores = "[{\"value\":1,\"text\":\"01\"},{\"value\":2,\"text\":\"02\"},{\"value\":3,\"text\":\"03\"},{\"value\":4,\"text\":\"04\"},{\"value\":5,\"text\":\"05\"},{\"value\":6,\"text\":\"06\"},{\"value\":7,\"text\":\"07\"},{\"value\":8,\"text\":\"08\"},{\"value\":9,\"text\":\"09\"},{\"value\":10,\"text\":\"10\"},{\"value\":11,\"text\":\"11\"},{\"value\":12,\"text\":\"12\"},{\"value\":13,\"text\":\"13\"},{\"value\":14,\"text\":\"14\"},{\"value\":15,\"text\":\"15\"},{\"value\":16,\"text\":\"16\"},{\"value\":17,\"text\":\"17\"},{\"value\":18,\"text\":\"18\"},{\"value\":19,\"text\":\"19\"},{\"value\":20,\"text\":\"20\"},{\"value\":21,\"text\":\"21\"},{\"value\":22,\"text\":\"22\"},{\"value\":23,\"text\":\"23\"},{\"value\":24,\"text\":\"24\"},{\"value\":25,\"text\":\"25\"},{\"value\":26,\"text\":\"26\"},{\"value\":27,\"text\":\"27\"},{\"value\":28,\"text\":\"28\"},{\"value\":29,\"text\":\"29\"},{\"value\":30,\"text\":\"30\"},{\"value\":31,\"text\":\"31\"}]"
                }
            );

            modelBuilder.Entity<Permissao>().HasData(
                new Permissao()
                {
                    Id = 11,
                    Nome = "Configurações",
                    Descricao = "Permite alterar configurações do sistema",
                    Regra = "configuracao",
                }
            );
        }
    }
}
