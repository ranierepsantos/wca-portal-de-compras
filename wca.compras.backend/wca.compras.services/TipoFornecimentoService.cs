using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wca.compras.domain.Dtos;
using wca.compras.domain.Entities;
using wca.compras.domain.Interfaces;
using wca.compras.domain.Interfaces.Services;
using wca.compras.domain.Util;

namespace wca.compras.services
{
    public class TipoFornecimentoService : ITipoFornecimentoService
    {
        private readonly IRepositoryManager _rm;
        private readonly IMapper _mapper;

        public TipoFornecimentoService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _rm = repositoryManager;
            _mapper = mapper;
        }

        public async Task<TipoFornecimentoDto> Create(CreateTipoFornecimentoDto tipo)
        {
            try
            {
                var data = _mapper.Map<TipoFornecimento>(tipo);

                await _rm.SaveAsync();

                return _mapper.Map<TipoFornecimentoDto>(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"TipoFornecimentoService.Create.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<IList<ListItem>> GetToList()
        {
            try
            {
                var itens = await _rm.TipoFornecimentoRepository.SelectAll()
                            .OrderBy(p => p.Nome).ToListAsync(); ;

                return _mapper.Map<IList<ListItem>>(itens);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"TipoFornecimentoService.GetToList.Error: {ex.Message}");
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
                Console.WriteLine($"TipoFornecimentoService.Update.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
    }
}
