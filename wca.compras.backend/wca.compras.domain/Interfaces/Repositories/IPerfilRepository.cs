using wca.compras.domain.Entities;

namespace wca.compras.domain.Interfaces.Repositories
{
    public interface IPerfilRepository: IRepository<Perfil>
    {
        public Task<PerfilPermissoes> GetWithPermissoesByIdAsync(string id);
        
    }
}
