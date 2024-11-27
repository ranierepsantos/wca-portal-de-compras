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
        public DbSet<Configuracao> Configuracoes { get; set; }

        public DbSet<UsuarioCentrodeCustos> UsuarioCentrodeCustos { get; set; }
        public DbSet<UsuarioConfiguracoes>  UsuarioConfiguracoes { get; set; }
        public DbSet<SolicitacaoFerias> SolicitacaoFerias { get; set; }
        public DbSet<TipoFerias> TipoFerias { get; set; }
        public DbSet<DocumentoComplementar> DocumentoComplementar { get; set; }
        public DbSet<Escala> Escalas { get; set; }
        internal DbSet<Escolaridade> Escolaridade { get; set; }
        public DbSet<Funcao> Funcoes { get; set; }
        public DbSet<Gestor> Gestores { get; set; }
        public DbSet<Horario> Horarios { get; set; }
        internal DbSet<MotivoContratacao> MotivoContratacao { get; set; }
        internal DbSet<Sexo> Sexo { get; set; }
        public DbSet<TipoContrato> TiposContrato { get; set; }
        internal DbSet<TipoFaturamento> TipoFaturamento { get; set; }
        internal DbSet<SolicitacaoVaga> Vagas { get; set; }
        internal DbSet<EventLogGi> EventLogGi { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsuarioClientes>().HasKey(pk => new { pk.UsuarioId, pk.ClienteId });

            modelBuilder.Entity<SolicitacaoDesligamento>().HasKey(pk => new { pk.SolicitacaoId });
            modelBuilder.Entity<SolicitacaoMudancaBase>().HasKey(pk => new { pk.SolicitacaoId });
            modelBuilder.Entity<SolicitacaoComunicado>().HasKey(pk => new { pk.SolicitacaoId });
            modelBuilder.Entity<SolicitacaoComunicado>().HasOne(o => o.Assunto);
            modelBuilder.Entity<SolicitacaoDesligamento>().HasOne(o => o.Motivo);
            modelBuilder.Entity<SolicitacaoMudancaBase>().HasMany(o => o.ItensMudanca).WithMany();
            modelBuilder.Entity<SolicitacaoFerias>().HasKey(pk => new { pk.SolicitacaoId });
            modelBuilder.Entity<SolicitacaoFerias>().HasOne(o => o.TipoFerias);
            modelBuilder.Entity<SolicitacaoVaga>().HasKey(pk => new { pk.SolicitacaoId });

            modelBuilder.Entity<Solicitacao>().HasOne(o => o.Desligamento);
            modelBuilder.Entity<Solicitacao>().HasOne(o => o.MudancaBase);
            modelBuilder.Entity<Solicitacao>().HasOne(o => o.Comunicado);
            modelBuilder.Entity<Solicitacao>().HasOne(o => o.Ferias);
            modelBuilder.Entity<Solicitacao>().HasOne(o => o.Vaga);


            modelBuilder.Entity<UsuarioCentrodeCustos>().HasKey(pk => new { pk.UsuarioId, pk.CentroCustoId });
            modelBuilder.Entity<UsuarioConfiguracoes>().HasKey(pk => new { pk.UsuarioId });

            modelBuilder.Entity<Funcionario>().HasIndex(ix =>  ix.eSocialMatricula).IsUnique();

            modelBuilder.Entity<SolicitacaoVaga>()
                .HasMany<DocumentoComplementar>(o => o.DocumentoComplementares).WithMany();

            modelBuilder.Entity<Funcionario>()
                .HasIndex(f => f.Nome);

        }
    }
}
