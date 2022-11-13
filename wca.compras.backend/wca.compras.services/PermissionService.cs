using wca.compras.domain.Dtos;
using wca.compras.domain.Entities;
using wca.compras.domain.Interfaces;
using wca.compras.domain.Interfaces.Services;
using wca.compras.domain.Util;
using static wca.compras.domain.Dtos.PermissionDtos;

namespace wca.compras.services
{
    public class PermissionService : IPermissionService
    {
        private readonly IRepository<Permission> _repository;
        
        public PermissionService(IRepository<Permission> permissionRepository)
        {
            _repository = permissionRepository;
        }

        public async Task<PermissionDto> Create(CreatePermissionDto permission)
        {
            var data = new Permission()
            {
                Name = permission.Name,
                Description = permission.Description,
                InternalName = permission.InternalName
            };

            await _repository.CreateAsync(data);
            return data.asDto();
        }

        public async Task<IList<PermissionDto>> GetAll()
        {
            var list = await _repository.GetAllAsync();

            return list.Select(p => p.asDto()).ToList();
        }

        public Task<PermissionDto> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<ListItem>> GetToList()
        {
            var itens = await _repository.GetAllAsync();

            var list = itens.OrderBy(p => p.Name).Select((p) => {
                return new ListItem() { Text = p.Name, Value = p.Id.ToString() };
            }).ToList();

            return list;
        }

        public Task Update(UpdatePermissionDto permisson)
        {
            throw new NotImplementedException();
        }
    }
}
