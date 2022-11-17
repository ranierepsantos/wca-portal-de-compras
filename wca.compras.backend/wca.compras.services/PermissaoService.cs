using AutoMapper;
using wca.compras.domain.Entities;
using wca.compras.domain.Interfaces;
using wca.compras.domain.Interfaces.Services;
using wca.compras.domain.Util;
using  wca.compras.domain.Dtos;

namespace wca.compras.services
{
    public class PermissaoService : IPermissaoService
    {
        private readonly IRepository<Permissao> _repository;
        private IMapper _mapper;

        public PermissaoService(IRepository<Permissao> permissionRepository, IMapper mapper)
        {
            _repository = permissionRepository;
            _mapper = mapper;
        }

        public async Task<PermissaoDto> Create(CreatePermissaoDto permissao)
        {
            var data = _mapper.Map<Permissao>(permissao);

            await _repository.CreateAsync(data);
            return _mapper.Map<PermissaoDto>(data);
        }

        public async Task<IList<PermissaoDto>> GetAll()
        {
            var list = await _repository.GetAllAsync();

            return _mapper.Map<IList<PermissaoDto>>(list);
        }

        public async Task<PermissaoDto> GetById(string id)
        {
            var data = await _repository.GetAsync(id);
            
            return _mapper.Map<PermissaoDto>(data);
        }

        public async Task<IList<ListItem>> GetToList()
        {
            var itens = await _repository.GetAllAsync();

            var list = _mapper.Map<IList<ListItem>>(itens.OrderBy(p => p.Nome).ToList());

            return list;
        }

        public Task<Pagination<PermissaoDto>> Paginate(int page, int pageSise)
        {
            throw new NotImplementedException();
        }

        public async Task<PermissaoDto?> Update(UpdatePermissaoDto permissao)
        {

            var baseData = await _repository.GetAsync(permissao.Id);

            if (baseData == null) return null;

            var data = _mapper.Map<Permissao>(permissao);

            await _repository.UpdateAsync(data);

            return _mapper.Map<PermissaoDto>(data);
            
        }
    }
}
