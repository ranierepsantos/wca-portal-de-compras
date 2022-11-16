using wca.compras.domain.Dtos;
using wca.compras.domain.Entities;
using wca.compras.domain.Interfaces;
using wca.compras.domain.Interfaces.Repositories;
using wca.compras.domain.Interfaces.Services;
using wca.compras.domain.Util;
using static wca.compras.domain.Dtos.PerfilDtos;

namespace wca.compras.services
{
    public class PerfilService : IPerfilService
    {
        private readonly IPerfilRepository _perfilRepo;
        private readonly IRepository<PerfilRelPermissoes> _perfilRelPermissaoRepo;

        public PerfilService(IPerfilRepository perfilRepository, IRepository<PerfilRelPermissoes> perfilPermissionRepository)
        {
            _perfilRepo = perfilRepository;
            _perfilRelPermissaoRepo = perfilPermissionRepository;
        }
        
        public async Task<PerfilDto> Create(CreatePerfilDto perfil)
        {

            var data = new Perfil()
            {
                Nome = perfil.Nome,
                Descricao = perfil.Descricao,
                Ativo = true
            };

            await _perfilRepo.CreateAsync(data);

            foreach(var permissao in perfil.Permissoes)
            {
                await _perfilRelPermissaoRepo.CreateAsync(new PerfilRelPermissoes()
                {
                    PerfilId = data.Id,
                    PermissaoId = permissao.Id,
                });
            }
            
            return data.asDto();
        }

        public async Task<IList<ListItem>> GetToList()
        {
            var itens = await _perfilRepo.GetAllAsync(p => p.Ativo == true);

            var list = itens.OrderBy(p => p.Nome).Select((p) => {
                return new ListItem() { Text = p.Nome, Value = p.Id.ToString() };
            }).ToList();

            return list;
        }

        public async Task<PerfilPermissoesDto> GetWithPermissoes(string id)
        {

            var perfil = await _perfilRepo.GetWithPermissoesByIdAsync(id);

            if (perfil == null) return null;

            return perfil.asDto();
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

            var data = new Perfil()
            {
                Id = perfil.Id,
                Nome = perfil.Nome,
                Descricao = perfil.Descricao,
                Ativo = perfil.Ativo
            };

            await _perfilRepo.UpdateAsync(data);

            return data.asDto();

        }
    }
}
