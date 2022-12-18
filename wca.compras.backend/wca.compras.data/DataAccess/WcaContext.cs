using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
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

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}
