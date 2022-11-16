using wca.compras.domain.Dtos;
using wca.compras.domain.Entities;
using wca.compras.domain.Interfaces;
using wca.compras.domain.Interfaces.Services;
using wca.compras.domain.Util;
using static wca.compras.domain.Dtos.PermissaoDtos;

namespace wca.compras.services
{
    public class PermissaoService : IPermissaoService
    {
        private readonly IRepository<Permissao> _repository;
        
        public PermissaoService(IRepository<Permissao> permissionRepository)
        {
            _repository = permissionRepository;
        }

        public async Task<PermissaoDto> Create(CreatePermissaoDto permissao)
        {
            var data = new Permissao()
            {
                Nome = permissao.Nome,
                Descricao = permissao.Descricao,
                Regra = permissao.Regra
            };

            await _repository.CreateAsync(data);
            return data.asDto();
        }

        public async Task<IList<PermissaoDto>> GetAll()
        {
            var list = await _repository.GetAllAsync();

            return list.Select(p => p.asDto()).ToList();
        }

        public Task<PermissaoDto> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<ListItem>> GetToList()
        {
            var itens = await _repository.GetAllAsync();

            var list = itens.OrderBy(p => p.Nome).Select((p) => {
                return new ListItem() { Text = p.Nome, Value = p.Id.ToString() };
            }).ToList();

            return list;
        }

        public async Task Update(UpdatePermissaoDto permissao)
        {

            var baseData = await _repository.GetAsync(permissao.Id);

            if (baseData == null) return;

            baseData.Nome = permissao.Nome;
            baseData.Descricao = permissao.Descricao;
            
            await _repository.UpdateAsync(baseData);
            
        }
    }
}
