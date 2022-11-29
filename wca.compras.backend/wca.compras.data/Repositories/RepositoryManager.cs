using wca.compras.data.DataAccess;
using wca.compras.domain.Entities;
using wca.compras.domain.Interfaces;

namespace wca.compras.data.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly WcaContext _context;
        private IRepository<Perfil> _perfilRepo;
        private IRepository<Permissao> _permissaoRepo;
        private IRepository<Usuario> _usuarioRepo;
        private IRepository<ResetPassword> _resetPassRepo;
        private IRepository<Cliente> _clienteRepo;

        public RepositoryManager(WcaContext context)
        {
            _context = context;
        }

        public IRepository<Perfil> PerfilRepository { 
            get
            {
                if (_perfilRepo == null)
                {
                    _perfilRepo = new BaseRepository<Perfil>(_context);
                }
                return _perfilRepo;
            }
        }

        public IRepository<Permissao> PermissaoRepository
        {
            get
            {
                if (_permissaoRepo == null)
                {
                    _permissaoRepo = new BaseRepository<Permissao>(_context);
                }
                return _permissaoRepo;
            }
        }

        public IRepository<Usuario> UsuarioRepository
        {
            get
            {
                if (_usuarioRepo == null)
                {
                    _usuarioRepo = new BaseRepository<Usuario>(_context);
                }
                return _usuarioRepo;
            }
        }

        public IRepository<ResetPassword> ResetPasswordRepository
        {
            get
            {
                if (_resetPassRepo == null)
                {
                    _resetPassRepo = new BaseRepository<ResetPassword>(_context);
                }
                return _resetPassRepo;
            }
        }

        public IRepository<Cliente> ClienteRepository
        {
            get
            {
                if (_clienteRepo == null)
                {
                    _clienteRepo = new BaseRepository<Cliente>(_context);
                }
                return _clienteRepo;
            }
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
