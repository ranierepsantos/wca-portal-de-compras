using AutoMapper;
using Microsoft.EntityFrameworkCore;
using wca.compras.domain.Dtos;
using wca.compras.domain.Entities;
using wca.compras.domain.Interfaces;
using wca.compras.domain.Interfaces.Services;
using wca.compras.domain.Util;

namespace wca.compras.services
{
    public class TipoFornecimentoervice : ITipoFornecimentoervice
    {
        private readonly IRepositoryManager _rm;
        private readonly IMapper _mapper;

        public TipoFornecimentoervice(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _rm = repositoryManager;
            _mapper = mapper;
        }

        public async Task<TipoFornecimentoDto> Create(CreateTipoFornecimentoDto tipo)
        {
            try
            {
                var data = _mapper.Map<TipoFornecimento>(tipo);

                _rm.TipoFornecimentoRepository.Create(data);

                await _rm.SaveAsync();

                return _mapper.Map<TipoFornecimentoDto>(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{this.GetType().Name}.Create.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<IList<ListItem>> GetToList()
        {
            try
            {
                var itens = await _rm.TipoFornecimentoRepository.SelectByCondition(c => c.Ativo)
                            .OrderBy(p => p.Nome).ToListAsync(); ;

                return _mapper.Map<IList<ListItem>>(itens);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"{this.GetType().Name}.GetToList.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
            
        }

        public async Task<TipoFornecimentoDto> Update(TipoFornecimentoDto updateTipo)
        {
            try
            {
                var baseData = await _rm.TipoFornecimentoRepository.SelectByCondition(p => p.Id == updateTipo.Id)
                            .FirstOrDefaultAsync();

                if (baseData == null) return null;

                 _mapper.Map(updateTipo, baseData);

                _rm.TipoFornecimentoRepository.Update(baseData);

                await _rm.SaveAsync();

                return _mapper.Map<TipoFornecimentoDto>(baseData);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"{this.GetType().Name}.Update.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public Pagination<TipoFornecimentoDto> Paginate(int page, int pageSize, string termo = "")
        {
            try
            {
                //Não trazer a MATRIZ
                var query = _rm.TipoFornecimentoRepository.SelectAll();

                if (!string.IsNullOrEmpty(termo))
                {
                    query = query.Where(q => q.Nome.Contains(termo));
                }
                query = query.OrderBy(p => p.Nome);

                var pagination = Pagination<TipoFornecimentoDto>.ToPagedList(_mapper, query, page, pageSize);

                return pagination;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{this.GetType().Name}.Paginate.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<IList<TipoFornecimentoDto>> GetByUser(int usuarioId)
        {
            try
            {
                var categorias = await _rm.TipoFornecimentoRepository.SelectByCondition(c =>  c.Ativo)
                    .Where(c => c.Usuario.Any(c => c.Id == usuarioId))
                    .ToListAsync();

                return _mapper.Map<IList<TipoFornecimentoDto>>(categorias);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{this.GetType().Name}.GetByUser.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
    }
}
