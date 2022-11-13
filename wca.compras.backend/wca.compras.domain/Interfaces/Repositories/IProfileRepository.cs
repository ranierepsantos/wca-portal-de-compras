using wca.compras.domain.Entities;

namespace wca.compras.domain.Interfaces.Repositories
{
    public interface IProfileRepository: IRepository<Profile>
    {
        public Task<ProfilePermissions> GetWithPermissionsById(string id);
        
    }
}
