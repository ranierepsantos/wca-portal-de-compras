using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using wca.compras.domain.Dtos;
using wca.compras.domain.Entities;
using wca.compras.domain.Interfaces;
using wca.compras.domain.Interfaces.Services;
using wca.compras.domain.Util;

namespace wca.compras.services
{
    public class RecorrenciaService : IRecorrenciaService
    {
        private readonly IRepositoryManager _rm;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private const int SEMANAL = 0;
        private const int MENSAL = 1;


        public RecorrenciaService(IMapper mapper,
            IRepositoryManager repositoryManager,
            IEmailService emailService)
        {
            _mapper = mapper;
            _rm = repositoryManager;
            _emailService = emailService;
        }
        public async Task<RecorrenciaDto> Create(CreateRecorrenciaDto createRecorrenciaDto, string urlOrigin)
        {
            try
            {
                var data = _mapper.Map<Recorrencia>(createRecorrenciaDto);
                data.UrlOrigin = urlOrigin;

                _rm.RecorrenciaRepository.Attach(data);

                foreach (var item in createRecorrenciaDto.RecorrenciaProdutos)
                {
                    var produto = _mapper.Map<RecorrenciaProduto>(item);
                    data.RecorrenciaProdutos.Add(produto);
                }

                await _rm.SaveAsync();

                return _mapper.Map<RecorrenciaDto>(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{this.GetType().Name}.Create.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<RecorrenciaDto> GetById(int filialId, int id)
        {
            try
            {
                var query = _rm.RecorrenciaRepository.SelectByCondition(p => p.Id == id);

                if (filialId > 1)
                    query = query.Where(c => c.FilialId == filialId);

                query = query.Include("Usuario")
                             .Include("Cliente")
                             .Include("Fornecedor")
                             .Include("RecorrenciaProdutos")
                             .Include("RecorrenciaLogs");

                var data = await query.FirstOrDefaultAsync();

                return _mapper.Map<RecorrenciaDto>(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{this.GetType().Name}.GetById.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public Pagination<RecorrenciaDto> Paginate(int filialId, int page = 1, int pageSize = 10, int clienteId = 0, int fornecedorId = 0, int usuarioId = 0)
        {
            try
            {
                var query = _rm.RecorrenciaRepository.SelectAll();

                if (filialId > 1)
                    query = query.Where(c => c.FilialId == filialId);

                if (clienteId > 0)
                    query = query.Where(c => c.ClienteId == clienteId);

                if (fornecedorId > 0)
                    query = query.Where(c => c.FornecedorId == fornecedorId);

                if (usuarioId > 0)
                    query = query.Where(c => c.UsuarioId == usuarioId);

                
                query = query.Include("Usuario")
                             .Include("Cliente")
                             .Include("Fornecedor");


                var pagination = Pagination<RecorrenciaDto>.ToPagedList(_mapper, query, page, pageSize);

                return pagination;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{this.GetType().Name}.Paginate.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<bool> Remove(int filialId, int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> EnabledDisabled(int filialId, EnabledRecorrenciaDto enabledRecorrenciaDto)
        {
            try
            {
                var query = _rm.RecorrenciaRepository.SelectByCondition(p => p.Id == enabledRecorrenciaDto.Id, true);

                if (filialId > 1)
                    query = query.Where(c => c.FilialId == filialId);

                var data = await query.FirstOrDefaultAsync();

                if (data == null) return false;

                data.Ativo = enabledRecorrenciaDto.Ativo;

                await _rm.SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{this.GetType().Name}.EnabledDisabled.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }


        public async Task<RecorrenciaDto> Update(int filialId, UpdateRecorrenciaDto updateRecorrenciaDto)
        {
            try
            {
                var query = _rm.RecorrenciaRepository.SelectByCondition(c => c.Id == updateRecorrenciaDto.Id);

                if (filialId > 1)
                {
                    query = query.Where(c =>  c.FilialId == filialId); 
                }
                query = query.Include(n => n.RecorrenciaProdutos);

                var baseData = await query.FirstOrDefaultAsync();

                if (baseData == null) return null;


                //Remover produtos caso tenha alterado
                baseData.RecorrenciaProdutos.ToList().ForEach(produto =>
                {
                    bool hasProduto = updateRecorrenciaDto.RecorrenciaProdutos.Where(p => p.Id == produto.Id).FirstOrDefault() != null;
                    if (hasProduto == false)
                    {
                        var item = _rm.RecorrenciaProdutoRepository.SelectByCondition(c => c.Id == produto.Id).FirstOrDefault();
                        if (item != null) _rm.RecorrenciaProdutoRepository.Delete(item);
                    }
                });

                _mapper.Map(updateRecorrenciaDto, baseData);

                _rm.RecorrenciaRepository.Update(baseData);

                await _rm.SaveAsync();

                return await GetById(filialId, updateRecorrenciaDto.Id);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"{this.GetType().Name}.Update.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<bool> ExecuteScheduleAsync()
        {
            int diaSemana = (int) DateTime.Now.DayOfWeek;
            int diaMes = DateTime.Now.Day;
            int ultimoDiaMes = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);

            string message = string.Empty;
            string status = "Sucesso";
            var requisicaoService = new RequisicaoService(_mapper, _rm, _emailService);

            var lista = await _rm.RecorrenciaRepository.SelectByCondition(
                    c=> c.Ativo == true && ((c.TipoRecorrencia == SEMANAL && c.Dia.Equals(diaSemana)) ||
                         (c.TipoRecorrencia == MENSAL && (c.Dia.Equals(diaMes)|| c.Dia > ultimoDiaMes))))
                .Include("RecorrenciaProdutos")
                .Include(n => n.Fornecedor)
                .Include(n => n.Cliente)
                .Include(n => n.Usuario)
                .ToListAsync();

            foreach (var item in lista )
            {
                decimal requisicaoValorTotal = 0;
                decimal requisicaoTaxaGestao = 0;

                // montar a lista de produtos
                List<RequisicaoItemDto> produtos = new List<RequisicaoItemDto>();
                foreach (var recorrenciaProduto in item.RecorrenciaProdutos) {
                    var produto = await _rm.ProdutoRepository.SelectByCondition(c => c.Codigo.Equals(recorrenciaProduto.Codigo) && c.FornecedorId == item.FornecedorId).FirstOrDefaultAsync();

                    if (produto != null)
                    {
                        var valorTotalProduto = (produto.TaxaGestao + produto.Valor + decimal.Parse((produto.Valor * produto.PercentualIPI/100).ToString("n2")) )  * recorrenciaProduto.Quantidade;
                    
                        produtos.Add(new RequisicaoItemDto(0, 0, produto.Codigo, produto.Nome, produto.UnidadeMedida, produto.Valor, produto.TaxaGestao, produto.PercentualIPI, recorrenciaProduto.Quantidade, valorTotalProduto, produto.TipoFornecimentoId));

                        requisicaoTaxaGestao += produto.TaxaGestao;
                        requisicaoValorTotal += valorTotalProduto;
                    }else
                    {
                        //produto não foi localizado - logar e não fazer o pedido?
                        status = "Insucesso";
                        message += message.IsNullOrEmpty() ? "Produto(s) não encontrado(s) <br>" : "";
                        message += $"{recorrenciaProduto.Codigo} - {recorrenciaProduto.Nome}. <br/>";
                    }
                }

                if (produtos.Count > 0 && message.IsNullOrEmpty())
                {
                    try
                    {
                        var valorICMS = decimal.Parse((requisicaoValorTotal * item.Fornecedor.Icms / 100).ToString("n2"));

                        var pedido = new CreateRequisicaoDto(item.FilialId, item.ClienteId, item.FornecedorId, requisicaoValorTotal, requisicaoTaxaGestao, item.Destino,
                                                             item.UsuarioId, item.Usuario.Nome, produtos, false, false,item.LocalEntrega, valorICMS,item.Fornecedor.Icms,item.PeriodoEntrega);

                        var requisicao = await requisicaoService.Create(pedido, item.UrlOrigin);

                        if (requisicao != null)
                        {
                            status = "Sucesso";
                            message = $"Requisição nº {requisicao.Id} criada com sucesso!";
                        }
                    }
                    catch (Exception ex)
                    {
                        status = "Erro";
                        message = "Erro ao criar requisição!";
                    }
                }

                var log = new RecorrenciaLog()
                {
                    Status = status,
                    Log = message,
                    RecorrenciaId = item.Id
                };
                _rm.RecorrenciaLogRepository.Create(log);
                await _rm.SaveAsync();
            }

            return true;
        }

        public async Task<RecorrenciaLogDto> Logs(int recorrenciaId)
        {
            try
            {
                var query = _rm.RecorrenciaLogRepository.SelectByCondition(c => c.RecorrenciaId == recorrenciaId);

                //query = query.OrderByDescending(o => o.DataHora);

                var log = await query.FirstOrDefaultAsync();

                if (log == null) return null;

                return _mapper.Map<RecorrenciaLogDto>(log);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{this.GetType().Name}.Paginate.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }


        public Pagination<RecorrenciaLogDto> PaginationLog(int recorrenciaId, int page = 1, int pageSize = 10)
        {
            try
            {
                var query = _rm.RecorrenciaLogRepository.SelectByCondition(c =>  c.RecorrenciaId == recorrenciaId);

                query = query.OrderByDescending(o => o.DataHora);

                var pagination = Pagination<RecorrenciaLogDto>.ToPagedList(_mapper, query, page, pageSize);

                return pagination;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{this.GetType().Name}.Paginate.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

    }
}
