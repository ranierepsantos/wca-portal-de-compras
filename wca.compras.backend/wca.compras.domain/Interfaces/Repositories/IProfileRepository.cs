using wca.compras.domain.Entities;

namespace wca.compras.domain.Interfaces.Repositories
{
    public interface IProfileRepository: IRepository<Perfil>
    {
        public Task<ProfilePermissions> GetWithPermissionsById(string id);
        
    }
}
