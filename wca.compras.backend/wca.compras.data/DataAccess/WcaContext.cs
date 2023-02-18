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
        }
    }
}
