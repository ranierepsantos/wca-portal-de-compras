using wca.compras.domain.Entities;

namespace wca.compras.domain.Interfaces
{
    public interface IRepositoryManager
    {
        public IRepository<Perfil> PerfilRepository { get; }
        public IRepository<Permissao> PermissaoRepository { get; }
        public IRepository<ResetPassword> ResetPasswordRepository { get; }
        public IRepository<Usuario> UsuarioRepository { get; }
        public IRepository<Cliente> ClienteRepository { get; }
        public IRepository<Filial> FilialRepository { get; }
        Task SaveAsync();
    }
}
