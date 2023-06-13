using AutoMapper;
using wca.compras.domain.Entities;
using wca.compras.domain.Interfaces;
using wca.compras.domain.Interfaces.Services;
using wca.compras.domain.Util;
using wca.compras.domain.Dtos;
using Microsoft.EntityFrameworkCore;

namespace wca.compras.services
{
    public class PermissaoService : IPermissaoService
    {
        private readonly IRepositoryManager _rm;
        private IMapper _mapper;

        public PermissaoService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _rm = repositoryManager;
            _mapper = mapper;
        }

        //public async Task<PermissaoDto> Create(CreatePermissaoDto permissao)
        //{
        //    var data = _mapper.Map<Permissao>(permissao);

        //    _rm.PermissaoRepository.Create(data);
        //    await _rm.SaveAsync();

        //    return _mapper.Map<PermissaoDto>(data);
        //}

        public async Task<IList<PermissaoDto>> GetAll(int sistemaId)
        {
            var list = await _rm.PermissaoRepository.SelectByCondition(q => q.SistemaId.Equals(sistemaId))
                       .OrderBy(p => p.Nome).ToListAsync();

            return _mapper.Map<IList<PermissaoDto>>(list);
        }

        //public async Task<PermissaoDto> GetById(int id)
        //{
        //    var data =  await _rm.PermissaoRepository.SelectByCondition(p => p.Id == id)
        //               .FirstOrDefaultAsync();

        //    return _mapper.Map<PermissaoDto>(data);
        //}

        public async Task<IList<ListItem>> GetToList(int sistemaId)
        {
            var itens = await _rm.PermissaoRepository.SelectByCondition(q => q.SistemaId.Equals(sistemaId))
                .OrderBy(p => p.Nome).ToListAsync(); ;

            return _mapper.Map<IList<ListItem>>(itens);
        }

        //public Pagination<PermissaoDto> Paginate(int page, int pageSize, string termo ="")
        //{
        //    var query = _rm.PermissaoRepository.SelectAll();

        //    if (!string.IsNullOrEmpty(termo))
        //    {
        //        query = query.Where(q => q.Nome.Contains(termo));
        //    }
        //    query = query.OrderBy(p => p.Nome);

        //    var pagination = Pagination<PermissaoDto>.ToPagedList(_mapper, query, page, pageSize);

        //    return pagination;
        //}

        //public async Task<PermissaoDto> Update(UpdatePermissaoDto permissao)
        //{

        //    var baseData = _rm.PermissaoRepository.SelectByCondition(p => p.Id == permissao.Id)
        //                    .FirstOrDefaultAsync();

        //    if (baseData == null) return null;

        //    var data = _mapper.Map<Permissao>(permissao);

        //    _rm.PermissaoRepository.Update(data);
        //    await _rm.SaveAsync();  

        //    return _mapper.Map<PermissaoDto>(data);
            
        //}
    }
}
