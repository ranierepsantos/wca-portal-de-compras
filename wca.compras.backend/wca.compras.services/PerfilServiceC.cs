using AutoMapper;
using Microsoft.EntityFrameworkCore;
using wca.compras.data.DataAccess;
using wca.compras.domain.Dtos;
using wca.compras.domain.Entities;
using wca.compras.domain.Interfaces;
using wca.compras.domain.Interfaces.Services;
using wca.compras.domain.Util;


namespace wca.compras.services
{
    public class PerfilServiceC : IPerfilService
    {
        private WcaContext _context;
        private IMapper _mapper;

        public PerfilServiceC(WcaContext context, IMapper mapper)
        {
            _context= context;
            _mapper = mapper;
        }
        
        public async Task<PerfilDto> Create(CreatePerfilDto perfil)
        {
            var data = _mapper.Map<Perfil>(perfil);
            
            _context.Perfis.Add(data);
            await _context.SaveChangesAsync();

            _context.Perfis.Attach(data);
            foreach (var permissao in perfil.Permissoes)
            {
                var perm = _mapper.Map<Permissao>(permissao);
                
                _context.Permissoes.Attach(perm);

                data.Permissao.Add(perm);
            }
            
            await _context.SaveChangesAsync();

            return _mapper.Map<PerfilDto>(data);
        }

        public async Task<IList<ListItem>> GetToList()
        {
            throw new NotImplementedException();
            //    var itens = await _rm.PerfilRepository.SelectByCondition(p => p.Ativo == true)
            //        .OrderBy(p => p.Nome)
            //        .ToListAsync();

            //    return _mapper.Map<IList<ListItem>>(itens); 
        }

        public async Task<PerfilPermissoesDto> GetWithPermissoes(string id)
        {
            throw new NotImplementedException();
            //var data = await _rm.PerfilRepository.SelectByCondition(p => p.Id == int.Parse(id))
            //    .Include(pm => pm.Permissao).FirstOrDefaultAsync();

            //var perfil = await _perfilRepo.GetWithPermissoesByIdAsync(id);

            //if (perfil == null) return null;

            //return _mapper.Map<PerfilPermissoesDto>(data);
        }

        public async Task<PerfilDto> Update(UpdatePerfilDto perfil)
        {



            //var bData = _rm.PerfilRepository.SelectByCondition(p => p.Id == perfil.Id)
            //    .Include(pm => pm.Permissao).ToListAsync();


            throw new NotImplementedException();

            //var baseData = _rm.PerfilRepository.SelectByCondition(p => p.Id == perfil.Id)
            //    .FirstOrDefault();

            //if (baseData == null)
            //{
            //    return null;
            //}

            ////Remover permissões caso tenha alterado
            //baseData.Permissoes.ForEach(async permissao =>
            //{
            //    var perm = perfil.Permissoes.Where(p => p.Id == permissao.Id).FirstOrDefault();
            //    if (perm == null)
            //    {
            //        var perfilRelPermissao = await _perfilRelPermissaoRepo.GetAsync(pp => pp.PermissaoId == permissao.Id && pp.PerfilId == perfil.Id);

            //        if (perfilRelPermissao != null)
            //            await _perfilRelPermissaoRepo.RemoveAsync(perfilRelPermissao.Id);
            //    }
            //});

            ////Adicionar permissões caso tenha novas
            //perfil.Permissoes.ToList().ForEach(async permissao =>
            //{
            //    if (baseData.Permissoes.Where(p => p.Id == permissao.Id).FirstOrDefault() == null)
            //    {
            //        await _perfilRelPermissaoRepo.CreateAsync(new PerfilRelPermissoes()
            //        {
            //            PerfilId = perfil.Id,
            //            PermissaoId = permissao.Id
            //        });
            //    }
            //});

            //var data = _mapper.Map<Perfil>(perfil);

            //await _perfilRepo.UpdateAsync(data);

            //return _mapper.Map<PerfilDto>(data);

        }

        public async Task<Pagination<PerfilDto>> Paginate(int page, int pageSize = 10, string termo = "")
        {
           throw new NotImplementedException();
        }
    }
}
