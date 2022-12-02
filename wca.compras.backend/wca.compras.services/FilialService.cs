using AutoMapper;

using wca.compras.domain.Dtos;
using wca.compras.domain.Interfaces;
using wca.compras.domain.Interfaces.Services;
using wca.compras.domain.Util;
using wca.compras.domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace wca.compras.services
{
    public class FilialService : IFilialService
    {
        private readonly IRepositoryManager _rm;
        private readonly IMapper _mapper;

        public FilialService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _rm = repositoryManager;
            _mapper = mapper;
        }

        public async Task<FilialDto> Create(CreateFilialDto createFilialDto)
        {
            try
            {
                var filial = _mapper.Map<Filial>(createFilialDto);
                _rm.FilialRepository.Create(filial);
                await _rm.SaveAsync();
                return _mapper.Map<FilialDto>(filial);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FilialService.Create.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<IList<FilialDto>> GetAll()
        {
            try
            {
                var filiais = await _rm.FilialRepository.SelectAll().OrderBy(f => f.Nome).ToListAsync();

                return _mapper.Map<IList<FilialDto>>(filiais);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FilialService.GetAll.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<FilialDto> GetById(int id)
        {
            try
            {
                var filial = await _rm.FilialRepository.SelectByCondition(c => c.Id == id).FirstOrDefaultAsync();

                if (filial == null) return null;

                return _mapper.Map<FilialDto>(filial);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"FilialService.GetById.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<IList<ListItem>> GetToList(int filialId)
        {
            try
            {
                var query = _rm.FilialRepository.SelectByCondition(c => c.Ativo == true);

                if (filialId > 1)
                {
                    query= query.Where(c => c.Id == filialId);
                }
                var itens = await query.OrderBy(p => p.Nome).ToListAsync(); ;

                return _mapper.Map<IList<ListItem>>(itens);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FilialService.GetToList.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public Pagination<FilialDto> Paginate(int page, int pageSize, string termo = "")
        {
            try
            {
                //Não trazer a MATRIZ
                var query = _rm.FilialRepository.SelectAll().Where(c => c.Id > 1);

                if (!string.IsNullOrEmpty(termo))
                {
                    query = query.Where(q => q.Nome.Contains(termo));
                }
                query = query.OrderBy(p => p.Nome);

                var pagination = Pagination<FilialDto>.ToPagedList(query, page, pageSize);

                return pagination;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FilialService.Paginate.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<FilialDto> Update(FilialDto filialDto)
        {
            try
            {
                var filial = await _rm.FilialRepository.SelectByCondition(c => c.Id == filialDto.Id).FirstOrDefaultAsync();

                if (filial == null) return null;

                _mapper.Map(filialDto, filial);
                
                _rm.FilialRepository.Update(filial);

                await _rm.SaveAsync();
                
                return filialDto;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FilialService.Update.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
    }
}
