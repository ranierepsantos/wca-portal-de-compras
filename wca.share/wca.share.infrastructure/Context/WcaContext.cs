using Microsoft.EntityFrameworkCore;
using wca.share.domain.Entities;

namespace wca.share.infrastruture.Context
{
    public class WcaContext : DbContext
    {
        public WcaContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Notificacao> Notificacoes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<UsuarioClientes> UsuariosClientes { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<SolicitacaoTipo> SolicitacaoTipo { get; set; }
        public DbSet<Solicitacao> Solicitacoes { get; set; }
        public DbSet<SolicitacaoDesligamento> SolicitacaoDesligamento { get; set; }
        public DbSet<SolicitacaoComunicado> SolicitacaoComunicado { get; set; }
        public DbSet<SolicitacaoMudancaBase> SolicitacaoMudancaBase { get; set; }
        public DbSet<SolicitacaoHistorico> SolicitacaoHistoricos { get; set; }
        public DbSet<Assunto> Assuntos { get; set; }    
        public DbSet<MotivoDemissao> MotivosDemissao { get; set; }    
        public DbSet<ItemMudanca> ItensMudanca { get; set; }
        public DbSet<StatusSolicitacao> StatusSolicitacao { get; set; }
        public DbSet<SolicitacaoArquivo> SolicitacaoArquivos { get; set; }
        public DbSet<Filial> Filiais { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsuarioClientes>().HasKey(pk => new { pk.UsuarioId, pk.ClienteId });

            modelBuilder.Entity<SolicitacaoDesligamento>().HasKey(pk => new { pk.SolicitacaoId });
            modelBuilder.Entity<SolicitacaoMudancaBase>().HasKey(pk => new { pk.SolicitacaoId });
            modelBuilder.Entity<SolicitacaoComunicado>().HasKey(pk => new { pk.SolicitacaoId });
            modelBuilder.Entity<SolicitacaoComunicado>().HasOne(o => o.Assunto);
            modelBuilder.Entity<SolicitacaoDesligamento>().HasOne(o => o.Motivo);
            modelBuilder.Entity<SolicitacaoMudancaBase>().HasMany(o => o.ItensMudanca).WithMany();

            modelBuilder.Entity<Solicitacao>().HasOne(o => o.Desligamento);
            modelBuilder.Entity<Solicitacao>().HasOne(o => o.MudancaBase);
            modelBuilder.Entity<Solicitacao>().HasOne(o => o.Comunicado);

        }
    }
}
