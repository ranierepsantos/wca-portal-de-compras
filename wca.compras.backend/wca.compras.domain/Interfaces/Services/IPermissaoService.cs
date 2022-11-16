using wca.compras.domain.Util;
using static wca.compras.domain.Dtos.PermissaoDtos;

namespace wca.compras.domain.Interfaces.Services
{
    public interface IPermissaoService
    {
        public Task<PermissaoDto> Create(CreatePermissaoDto permission);
        public Task Update(UpdatePermissaoDto permisson);
        public Task<IList<PermissaoDto>> GetAll();
        public Task<PermissaoDto> GetById(Guid id);
        public Task<IList<ListItem>> GetToList();
    }
}
