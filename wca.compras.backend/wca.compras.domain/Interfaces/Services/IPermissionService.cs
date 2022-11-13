using wca.compras.domain.Util;
using static wca.compras.domain.Dtos.PermissionDtos;

namespace wca.compras.domain.Interfaces.Services
{
    public interface IPermissionService
    {
        public Task<PermissionDto> Create(CreatePermissionDto permission);
        public Task Update(UpdatePermissionDto permisson);
        public Task<IList<PermissionDto>> GetAll();
        public Task<PermissionDto> GetById(Guid id);
        public Task<IList<ListItem>> GetToList();
    }
}
