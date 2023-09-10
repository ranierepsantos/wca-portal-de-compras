using Microsoft.EntityFrameworkCore;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.infrastruture.Context
{
    public class WcaReembolsoContext : DbContext
    {
        public WcaReembolsoContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<ContaCorrente> ContaCorrente { get; set; }
        public DbSet<Despesa> Despesas { get; set; }
        public DbSet<Faturamento> Faturamentos { get; set; }
        public DbSet<FaturamentoItem> FaturamentoItems { get; set; }
        public DbSet<Notificacao> Notificacoes { get; set; }
        public DbSet<Solicitacao> Solicitacoes { get; set; }
        public DbSet<SolicitacaoHistorico> SolicitacaoHistorico { get; set; }
        public DbSet<StatusSolicitacao> StatusSolicitacao { get; set; }
        public DbSet<TipoDespesa> TipoDespesas { get; set; }
        public DbSet<Transacao> Transacoes { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<UsuarioClientes> UsuariosClientes { get; set; }

        //public DbSet<Filial> Filial { get; set; }
        //public DbSet<FilialUsuario> FilialUsuario { get; set; }
        
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsuarioClientes>().HasKey(pk => new { pk.UsuarioId, pk.ClienteId });

            modelBuilder.Entity<ContaCorrente>()
                .HasKey(pk => new { pk.UsuarioId });

            modelBuilder.Entity<ContaCorrente>()
                .HasMany(c => c.Transacoes)
                .WithOne(t => t.ContaCorrente)
                .HasPrincipalKey(t => t.UsuarioId);

            //modelBuilder.Entity<FilialUsuario>().HasKey(pk => new { pk.UsuarioId, pk.FilialId });
        }
    }
}
