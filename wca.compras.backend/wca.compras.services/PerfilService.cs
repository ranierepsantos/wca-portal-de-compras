using AutoMapper;
using wca.compras.domain.Dtos;
using wca.compras.domain.Entities;
using wca.compras.domain.Interfaces;
using wca.compras.domain.Interfaces.Repositories;
using wca.compras.domain.Interfaces.Services;
using wca.compras.domain.Util;


namespace wca.compras.services
{
    public class PerfilService : IPerfilService
    {
        private readonly IPerfilRepository _perfilRepo;
        private readonly IRepository<PerfilRelPermissoes> _perfilRelPermissaoRepo;
        private IMapper _mapper;

        public PerfilService(IPerfilRepository perfilRepository, IRepository<PerfilRelPermissoes> perfilPermissionRepository, IMapper mapper)
        {
            _perfilRepo = perfilRepository;
            _perfilRelPermissaoRepo = perfilPermissionRepository;
            _mapper = mapper;
        }
        
        public async Task<PerfilDto> Create(CreatePerfilDto perfil)
        {

            var data = _mapper.Map<Perfil>(perfil);

            await _perfilRepo.CreateAsync(data);

            foreach(var permissao in perfil.Permissoes)
            {
                await _perfilRelPermissaoRepo.CreateAsync(new PerfilRelPermissoes()
                {
                    PerfilId = data.Id,
                    PermissaoId = permissao.Id,
                });
            }
            
            return _mapper.Map<PerfilDto>(data);
        }

        public async Task<IList<ListItem>> GetToList()
        {
            var itens = await _perfilRepo.GetAllAsync(p => p.Ativo == true);
            
            return _mapper.Map<IList<ListItem>>(itens.OrderBy(p => p.Nome).ToList()); 
        }

        public async Task<PerfilPermissoesDto> GetWithPermissoes(string id)
        {

            var perfil = await _perfilRepo.GetWithPermissoesByIdAsync(id);

            if (perfil == null) return null;

            return _mapper.Map<PerfilPermissoesDto>(perfil);
        }

        public async Task<PerfilDto> Update(UpdatePerfilDto perfil)
        {
            var baseData = await _perfilRepo.GetWithPermissoesByIdAsync(perfil.Id);

            if (baseData == null)
            {
                return null;
            }

            //Remover permissões caso tenha alterado
            baseData.Permissoes.ForEach(async permissao =>
            {
                var perm = perfil.Permissoes.Where(p => p.Id == permissao.Id).FirstOrDefault();
                if (perm == null)
                {
                    var perfilRelPermissao = await _perfilRelPermissaoRepo.GetAsync(pp => pp.PermissaoId == permissao.Id && pp.PerfilId == perfil.Id);

                    if (perfilRelPermissao != null)
                        await _perfilRelPermissaoRepo.RemoveAsync(perfilRelPermissao.Id);
                }
            });

            //Adicionar permissões caso tenha novas
            perfil.Permissoes.ToList().ForEach(async permissao =>
            {
                if (baseData.Permissoes.Where(p => p.Id == permissao.Id).FirstOrDefault() == null)
                {
                    await _perfilRelPermissaoRepo.CreateAsync(new PerfilRelPermissoes() { 
                            PerfilId = perfil.Id,
                            PermissaoId = permissao.Id
                        });
                }
            });

            var data = _mapper.Map<Perfil>(perfil);

            await _perfilRepo.UpdateAsync(data);

            return _mapper.Map<PerfilDto>(data);

        }
    }
}
