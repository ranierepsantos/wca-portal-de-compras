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
        public DbSet<UsuarioClientes> UsuariosClientes { get;set; }
        public DbSet<TipoDespesa> TipoDespesas { get;set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsuarioClientes>().HasKey(pk => new { pk.UsuarioId, pk.ClienteId });
        }
    }
}
