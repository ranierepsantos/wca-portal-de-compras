using wca.compras.domain.Util;
using wca.compras.domain.Dtos;

namespace wca.compras.domain.Interfaces.Services
{
    public interface IPermissaoService
    {
        public Task<PermissaoDto> Create(CreatePermissaoDto permission);
        public Task<PermissaoDto> Update(UpdatePermissaoDto permisson);
        public Task<IList<PermissaoDto>> GetAll();
        public Task<PermissaoDto> GetById(string id);
        public Task<IList<ListItem>> GetToList();
    }
}
