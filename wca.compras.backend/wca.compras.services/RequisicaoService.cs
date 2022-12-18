using AutoMapper;
using Microsoft.EntityFrameworkCore;
using wca.compras.domain.Dtos;
using wca.compras.domain.Entities;
using wca.compras.domain.Interfaces;
using wca.compras.domain.Interfaces.Services;
using wca.compras.domain.Util;

namespace wca.compras.services
{
    public class RequisicaoService: IRequisicaoService
    {
        private readonly IRepositoryManager _rm;
        private readonly IMapper _mapper;

        public RequisicaoService(IMapper mapper, IRepositoryManager repositoryManager)
        {
            _mapper = mapper;
            _rm = repositoryManager;
        }

        
        public async Task<RequisicaoDto> Create(int usuarioId, CreateRequisicaoDto createRequisicaoDto)
        {
            try
            {
                var data = _mapper.Map<Requisicao>(createRequisicaoDto);
                
                data.UsuarioId = usuarioId;

                _rm.RequisicaoRepository.Attach(data);

                foreach (var item in createRequisicaoDto.RequisicaoItens)
                {
                    var produto = _mapper.Map<RequisicaoItem>(item);
                    data.RequisicaoItens.Add(produto);
                }

                await _rm.SaveAsync();

                RequisicaoHistorico reqH = new RequisicaoHistorico()
                {
                    RequisicaoId = data.Id,
                    UsuarioId = usuarioId,
                    Evento = "Requisição criada",
                    DataHora= DateTime.Now
                };

                await CreateRequisicaoHistorico(reqH);

                return _mapper.Map<RequisicaoDto>(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{this.GetType().Name}.Create.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<RequisicaoDto> GetById(int filialId, int id)
        {
            try
            {
                var query = _rm.RequisicaoRepository.SelectByCondition(p => p.Id == id);

                if (filialId > 1)
                    query = query.Where(c => c.FilialId == filialId);

                query = query.Include("Usuario")
                             .Include("Cliente")
                             .Include("Fornecedor")
                             .Include("RequisicaoItens")
                             .Include(r => r.RequisicaoHistorico)
                                .ThenInclude(rh => rh.Usuario);

                var data = await query.FirstOrDefaultAsync();

                return _mapper.Map<RequisicaoDto>(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{this.GetType().Name}.GetById.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public Pagination<RequisicaoDto> Paginate(int filialId, int page = 1, int pageSize = 10, int clienteId = 0, int fornecedorId = 0, int usuarioId = 0, EnumStatusRequisicao status = EnumStatusRequisicao.TODOS)
        {
            try
            {
                var query = _rm.RequisicaoRepository.SelectAll();

                if (filialId > 1)
                    query = query.Where(c => c.FilialId == filialId);

                if (clienteId > 0)
                    query = query.Where(c => c.ClienteId == clienteId);

                if (fornecedorId > 0)
                    query = query.Where(c => c.FornecedorId == fornecedorId);

                if (usuarioId > 0)
                    query = query.Where(c => c.UsuarioId == usuarioId);

                if (status != EnumStatusRequisicao.TODOS)
                    query = query.Where(c => c.Status == status);


                query = query.Include("Usuario")
                             .Include("Cliente")
                             .Include("Fornecedor");
                             

                var pagination = Pagination<RequisicaoDto>.ToPagedList(_mapper, query, page, pageSize);
                
                return pagination;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{this.GetType().Name}.GetById.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public Task<bool> Remove(int filialId, int id)
        {
            throw new NotImplementedException();
        }

        public async Task<RequisicaoDto> Update(int filialId, int usuarioId, UpdateRequisicaoDto updateRequisicaoDto)
        {
            try
            {
                var query = _rm.RequisicaoRepository.SelectByCondition(p => p.Id == updateRequisicaoDto.Id);

                if (filialId > 1)
                    query = query.Where(c => c.FilialId == filialId);

                query = query.Include("RequisicaoItens");

                var data = await query.FirstOrDefaultAsync();

                if (data == null) return null;

                //Remover produtos caso tenha alterado
                data.RequisicaoItens.ToList().ForEach(produto =>
                {
                    bool hasProduto = updateRequisicaoDto.RequisicaoItens.Where(p => p.Id == produto.Id).FirstOrDefault() != null;
                    if (hasProduto == false )
                    {
                        var item = _rm.RequisicaoItemRepository.SelectByCondition(c =>  c.Id == produto.Id).FirstOrDefault();
                        if (item != null) _rm.RequisicaoItemRepository.Delete(item);
                    }
                });

                _mapper.Map(updateRequisicaoDto, data);
                
                _rm.RequisicaoRepository.Update(data);

                await _rm.SaveAsync();

                await CreateRequisicaoHistorico(new RequisicaoHistorico() {
                    DataHora = DateTime.Now,
                    Evento = "Requisição alterada",
                    UsuarioId = usuarioId,
                    RequisicaoId = data.Id
                });

                return _mapper.Map<RequisicaoDto>(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{this.GetType().Name}.Update.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }


        private async Task CreateRequisicaoHistorico(RequisicaoHistorico historico)
        {
            try
            {
                _rm.RequisicaoHistoricoRepository.Create(historico);
                await _rm.SaveAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{this.GetType().Name}.CreateRequisicaoHistorico.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
    }
}
