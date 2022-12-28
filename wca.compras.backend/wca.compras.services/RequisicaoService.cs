using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
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
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public RequisicaoService(IMapper mapper, 
            IRepositoryManager repositoryManager,
            IEmailService emailService)
        {
            _mapper = mapper;
            _rm = repositoryManager;
            _emailService = emailService;
        }

        
        public async Task<RequisicaoDto> Create(CreateRequisicaoDto createRequisicaoDto)
        {
            try
            {
                var data = _mapper.Map<Requisicao>(createRequisicaoDto);
                
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
                    Evento = $"Requisição criada por {createRequisicaoDto.NomeUsuario}",
                    DataHora= DateTime.Now
                };

                await CreateRequisicaoHistorico(reqH);

                /* checar se houve "estouro do limite de compra por categoria
                 * sim => enviar e-mail para adm ou cliente aprovar
                 * não => enviar e-mail para o fornecedor
                */ 

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
                //_emailService.SendRequisicaoFornecedorEmail();

                var query = _rm.RequisicaoRepository.SelectByCondition(p => p.Id == id);

                if (filialId > 1)
                    query = query.Where(c => c.FilialId == filialId);

                query = query.Include("Usuario")
                             .Include("Cliente")
                             .Include("Fornecedor")
                             .Include("RequisicaoItens")
                             .Include(r => r.RequisicaoHistorico);
                               

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

        public async Task<RequisicaoDto> Update(int filialId, UpdateRequisicaoDto updateRequisicaoDto)
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
                    Evento = $"Requisição alterada por ${updateRequisicaoDto.NomeUsuario}",
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

        private string randomTokenString()
        {
            var randomBytes = new byte[40];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(randomBytes);
            }
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }
    }
}
