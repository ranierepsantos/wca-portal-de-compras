using wca.compras.domain.Util;
using wca.compras.domain.Dtos;
using wca.compras.domain.Entities;

namespace wca.compras.domain.Interfaces.Services
{
    public interface IPermissaoService
    {
        public Task<PermissaoDto> Create(CreatePermissaoDto permission);
        public Task<PermissaoDto> Update(UpdatePermissaoDto permisson);
        public Task<IList<PermissaoDto>> GetAll();
        public Task<PermissaoDto> GetById(int id);
        public Task<IList<ListItem>> GetToList();
        public Pagination<PermissaoDto> Paginate(int page, int pageSize, string termo = "");
    }
}
