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
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsuarioClientes>().HasKey(pk => new { pk.UsuarioId, pk.ClienteId });
        }
    }
}
