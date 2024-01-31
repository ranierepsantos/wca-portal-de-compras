using Microsoft.EntityFrameworkCore;
using wca.share.application.Contracts.Persistence;
using wca.share.domain.Entities;
using wca.share.infrastruture.Context;

namespace wca.share.infrastruture.Persistence
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly WcaContext _context;
        private IRepository<Cliente>? _clienteRepo;
        private IRepository<Notificacao>? _notificacaoRepo;
        private IRepository<UsuarioClientes>? _usuarioClienteRepo;
        private IRepository<Usuario>? _usuarioRepo;
        
        public RepositoryManager(WcaContext context) => _context = context;

        public IRepository<Cliente> ClienteRepository => _clienteRepo ??= new BaseRepository<Cliente>(_context);

        public IRepository<Notificacao> NotificacaoRepository => _notificacaoRepo ??= new BaseRepository<Notificacao>(_context);

        public IRepository<UsuarioClientes> UsuarioClientesRepository => _usuarioClienteRepo ??= new BaseRepository<UsuarioClientes>(_context);
        public IRepository<Usuario> UsuarioRepository => _usuarioRepo ??= new BaseRepository<Usuario>(_context);
        
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<int> ExecuteCommandAsync(string command)
        {
            int result = await _context.Database.ExecuteSqlRawAsync(command);
            return result;
        }
        public DbSet<T> GetDbSet<T>() where T : class
        {
            return _context.Set<T>();
        }
    }
}
