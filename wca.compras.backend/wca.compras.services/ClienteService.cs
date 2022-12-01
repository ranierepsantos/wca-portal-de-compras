using AutoMapper;
using Microsoft.EntityFrameworkCore;
using wca.compras.domain.Dtos;
using wca.compras.domain.Entities;
using wca.compras.domain.Interfaces;
using wca.compras.domain.Interfaces.Services;
using wca.compras.domain.Util;

namespace wca.compras.services
{
    public class ClienteService : IClienteService
    {
        private readonly IRepositoryManager _rm;
        private readonly IMapper _mapper;

        public ClienteService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _rm = repositoryManager;
            _mapper = mapper;
        }

        public async Task<ClienteDto> Create(CreateClienteDto cliente)
        {
            try
            {
                var data = _mapper.Map<Cliente>(cliente);
                _rm.ClienteRepository.Attach(data);

                foreach(var item in cliente.ClienteContatos)
                {
                    var contato = _mapper.Map<ClienteContato>(item);
                    data.ClienteContatos.Add(contato);
                }

                foreach (var item in cliente.ClienteOrcamentoConfiguracao)
                {
                    var orcamentoConfiguracao = _mapper.Map<ClienteOrcamentoConfiguracao>(item);
                    data.ClienteOrcamentoConfiguracao.Add(orcamentoConfiguracao);
                }
                
                await _rm.SaveAsync(); 

                return _mapper.Map<ClienteDto>(data);
            }   
            catch (Exception ex)
            {
                Console.WriteLine($"ClienteService.Create.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<IList<ClienteDto>> GetAll(int filialId)
        {
            try
            {
                IList<Cliente> clientes;
                var query = _rm.ClienteRepository.SelectAll();

                if (filialId > 1)
                    query = query.Where(c => c.FilialId == filialId);

                clientes = await query.OrderBy(c => c.Nome).ToArrayAsync();

                return _mapper.Map<IList<ClienteDto>>(clientes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ClienteService.Create.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<ClienteDto> GetById(int filialId, int id)
        {
            try
            {
                var query = _rm.ClienteRepository.SelectByCondition(p => p.Id == id);

                if (filialId > 1)
                    query = query.Where(c => c.FilialId == filialId);

                query = query.Include(cc => cc.ClienteContatos)
                             .Include(co => co.ClienteOrcamentoConfiguracao);

                var data = await query.FirstOrDefaultAsync();

                return _mapper.Map<ClienteDto>(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ClienteService.Create.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
            
        }

        public Task<Pagination<ClienteDto>> Paginate(int filialId, int page = 1, int pageSize = 10, string termo = "")
        {
            throw new NotImplementedException();
        }

        public Task<bool> Remove(int filialId, int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ClienteDto> Update(int filialId, UpdateClienteDto cliente)
        {
            try
            {
                var query = _rm.ClienteRepository.SelectByCondition(p => p.Id == cliente.Id);

                if (filialId > 1)
                    query = query.Where(c => c.FilialId == filialId);

                query = query.Include(cc => cc.ClienteContatos)
                             .Include(co => co.ClienteOrcamentoConfiguracao);

                var baseData = await query.FirstOrDefaultAsync();
                
                if (baseData == null)
                {
                    return null;
                }

                _mapper.Map(cliente, baseData);
                _rm.ClienteRepository.Update(baseData);
                await _rm.SaveAsync();

                return _mapper.Map<ClienteDto>(baseData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ClienteService.Create.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
    }
}
