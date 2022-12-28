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
                Console.WriteLine($"ClienteService.GetAll.Error: {ex.Message}");
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
                Console.WriteLine($"ClienteService.GetById.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
            
        }

        public async Task<IList<ListItem>> GetToList(int filialId)
        {
            try
            {
                var query = _rm.ClienteRepository.SelectByCondition(c => c.Ativo == true);

                if (filialId > 0)
                {
                    query = query.Where(c => c.FilialId == filialId);
                }

                var itens = await query.OrderBy(p => p.Nome).ToListAsync(); ;

                return _mapper.Map<IList<ListItem>>(itens);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ClienteService.GetToList.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public Pagination<ClienteDto> Paginate(int filialId, int page = 1, int pageSize = 10, string termo = "")
        {
            try
            {
                var query = _rm.ClienteRepository.SelectAll();
                //Matriz (id: 1) retorna todos os dados
                if (filialId > 1)
                {
                    query = query.Where(c => c.FilialId ==filialId);
                }

                if (!string.IsNullOrEmpty(termo))
                {
                    query = query.Where(q => q.Nome.Contains(termo));
                }
                query = query.OrderBy(p => p.Nome);

                var pagination = Pagination<ClienteDto>.ToPagedList(_mapper, query, page, pageSize);

                return pagination;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ClienteService.Paginate.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public Task<bool> Remove(int filialId, int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ClienteDto> Update(int filialId, UpdateClienteDto cliente)
        {
            try
            {
                var query = _rm.ClienteRepository.SelectByCondition(p => p.Id == cliente.Id,false);

                if (filialId > 1)
                    query = query.Where(c => c.FilialId == filialId);

                query = query.Include(cc => cc.ClienteContatos)
                             .Include(co => co.ClienteOrcamentoConfiguracao);

                var baseData = await query.FirstOrDefaultAsync();
                
                if (baseData == null)
                {
                    return null;
                }

                //Remover contatos caso tenha alterado
                baseData.ClienteContatos.ToList().ForEach(contato =>
                {
                    var c = cliente.ClienteContatos.Where(p => p.Id == contato.Id).FirstOrDefault();
                    if (c == null)
                    {
                        var ct = _rm.ClienteContatoRepository.SelectByCondition(p => p.Id == contato.Id).FirstOrDefault();
                        if (ct != null) _rm.ClienteContatoRepository.Delete(ct);
                    }
                });

                _mapper.Map(cliente, baseData);
                _rm.ClienteRepository.Update(baseData);
                
                await _rm.SaveAsync();

                return _mapper.Map<ClienteDto>(baseData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ClienteService.Update.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
    }
}
