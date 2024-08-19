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
        private IRepository<Solicitacao>? _solicitacaoRepo;
        private IRepository<ItemMudanca>? _itemMudancaRepo;
        private IRepository<SolicitacaoHistorico>? _solicitacaoHistoricoRepo;
        private IRepository<SolicitacaoMudancaBase>? _mudancaBaseRepo;

        public RepositoryManager(WcaContext context) => _context = context;

        public IRepository<Cliente> ClienteRepository => _clienteRepo ??= new BaseRepository<Cliente>(_context);

        public IRepository<Notificacao> NotificacaoRepository => _notificacaoRepo ??= new BaseRepository<Notificacao>(_context);

        public IRepository<UsuarioClientes> UsuarioClientesRepository => _usuarioClienteRepo ??= new BaseRepository<UsuarioClientes>(_context);
        public IRepository<Usuario> UsuarioRepository => _usuarioRepo ??= new BaseRepository<Usuario>(_context);
        public IRepository<Solicitacao> SolicitacaoRepository => _solicitacaoRepo ??= new BaseRepository<Solicitacao>(_context);

        public IRepository<ItemMudanca> ItemMudancaRepository => _itemMudancaRepo ??= new BaseRepository<ItemMudanca>(_context);
        public IRepository<SolicitacaoHistorico> SolicitacaoHistoricoRepository => _solicitacaoHistoricoRepo ??= new BaseRepository<SolicitacaoHistorico>(_context);

        public IRepository<SolicitacaoMudancaBase> MudancaBaseRepository => _mudancaBaseRepo??= new BaseRepository<SolicitacaoMudancaBase>(_context);

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

        public IQueryable<T> FromQuery<T>(string query) where T : class
        {
            return _context.Set<T>().FromSqlRaw(query);
        }
    }
}
