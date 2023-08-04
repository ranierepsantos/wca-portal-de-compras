using AutoMapper;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using wca.compras.domain.Dtos;
using wca.compras.domain.Entities;
using wca.compras.domain.Interfaces;
using wca.compras.domain.Interfaces.Services;
using wca.compras.domain.Util;

namespace wca.compras.services
{
    public class RequisicaoService : IRequisicaoService
    {
        private readonly IRepositoryManager _rm;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly List<Configuracao> _configuracoes;
        private readonly TimeZoneInfo _timeZoneBrasilia;

        public RequisicaoService(IMapper mapper,
            IRepositoryManager repositoryManager,
            IEmailService emailService)
        {
            _mapper = mapper;
            _rm = repositoryManager;
            _emailService = emailService;
            _timeZoneBrasilia = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            _configuracoes = _rm.ConfiguracaoRepository.SelectAll().ToList();

        }

        public async Task<RequisicaoDto> Create(CreateRequisicaoDto createRequisicaoDto, string urlOrigin = "")
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

                if (data.RequerAutorizacaoWCA == false || data.RequerAutorizacaoCliente == false)
                {

                    var cliente = await _rm.ClienteRepository.SelectByCondition(c => c.Id == data.ClienteId)
                                           .Include(inc => inc.ClienteOrcamentoConfiguracao)
                                           .FirstOrDefaultAsync();

                    //verificar o foi excedido a quantidade de pedidos determinadas pro mês
                    if (cliente?.ClienteOrcamentoConfiguracao.Count() > 0)
                    {
                        var (dataIni, dataFim) = getDataCorte();

                        var comprasMes = await GetQuantidadePedidoPorCliente((int)data.ClienteId, dataIni, dataFim);

                        foreach (var item in cliente.ClienteOrcamentoConfiguracao)
                        {
                            if (item.Ativo && comprasMes[item.TipoFornecimentoId] > item.QuantidadeMes && item.QuantidadeMes > 0)
                            {
                                if (item.AprovadoPor == EnumAprovadoPor.WCA)
                                    data.RequerAutorizacaoWCA = true;
                                else
                                    data.RequerAutorizacaoCliente = true;
                            }
                        }
                    }
                    //verificar se o pedido ultrapassa o valor limite 
                    if (cliente.NaoUltrapassarLimitePorRequisicao && cliente.ValorLimiteRequisicao < data.ValorTotal)
                    {
                        data.RequerAutorizacaoWCA = true;
                    }

                }

                if (!string.IsNullOrEmpty(createRequisicaoDto.UsuarioAutorizador))
                {
                    data.RequerAutorizacaoWCA = false;
                }

                if (data.RequerAutorizacaoCliente == false && data.RequerAutorizacaoWCA == false)
                    data.Status = EnumStatusRequisicao.APROVADO;

                await _rm.SaveAsync();


                string mensagemEvento = $"Requisição criada por {createRequisicaoDto.NomeUsuario}";
                mensagemEvento += string.IsNullOrEmpty(createRequisicaoDto.UsuarioAutorizador) ? "." : $" e autorizada por {createRequisicaoDto.UsuarioAutorizador}.";


                RequisicaoHistorico reqH = new RequisicaoHistorico()
                {
                    RequisicaoId = data.Id,
                    Evento = mensagemEvento,
                    DataHora = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _timeZoneBrasilia)
                };



                await CreateRequisicaoHistorico(reqH);

                if (data.RequerAutorizacaoCliente == true)
                {
                    await solicitarAprovacaoCliente(urlOrigin, data);
                }

                if (data.RequerAutorizacaoWCA == true)
                {
                    await solicitarAprovacaoWCA(urlOrigin, data);
                }

                if (data.RequerAutorizacaoWCA == false && data.RequerAutorizacaoCliente == false)
                {
                    await solicitarAprovacaoFornecedor(urlOrigin, data);
                }

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
                             .Include(r => r.Cliente)
                             .ThenInclude(c => c.ClienteOrcamentoConfiguracao)
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

        public async Task<RequisicaoAprovacaoDto> GetByAprovacaoToken(string tokenAprovacao)
        {
            try
            {
                var requisicaoAprovacao = await _rm.RequisicaoAprovacaoRepository
                     .SelectByCondition(c => c.TokenAprovador == tokenAprovacao)
                     .FirstOrDefaultAsync();

                if (requisicaoAprovacao == null || !requisicaoAprovacao.Ativo)
                    throw new Exception("Token inválido e/ou expirado!");

                var query = _rm.RequisicaoRepository.SelectByCondition(p => p.Id == requisicaoAprovacao.RequisicaoId);
                query = query.Include("Usuario")
                             .Include("Cliente")
                             .Include("RequisicaoItens");

                var data = await query.FirstOrDefaultAsync();

                if (data == null)
                {
                    return null;
                }
                string localEntrega = $"{data.Endereco}, {data.Numero} - CEP: {data.Cep} - {data.Cidade} / {data.UF}";

                var dto = new RequisicaoAprovacaoDto(
                    data.Id,
                    data.ValorTotal,
                    data.TaxaGestao,
                    data.Destino,
                    data.Cliente,
                    _mapper.Map<ListItem>(data.Usuario),
                    _mapper.Map<IList<RequisicaoItemDto>>(data.RequisicaoItens),
                    requisicaoAprovacao.NomeAprovador,
                    localEntrega,
                    data.PeriodoEntrega
                );
                return dto;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{this.GetType().Name}.GetByAprovacaoToken.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<bool> aprovarRequisicao(AprovarRequisicaoDto aprovarRequisicaoDto, string urlOrigin)
        {
            try
            {
                bool alterarStatus = true;
                int requisicaoId = aprovarRequisicaoDto.Id;
                EnumTipoAprovador tipoAprovador = EnumTipoAprovador.WCA;

                if (requisicaoId == 0 && aprovarRequisicaoDto.Token.ToUpper() != "TELAEDICAO")
                {
                    var requisicaoAprovacao = await _rm.RequisicaoAprovacaoRepository
                     .SelectByCondition(c => c.TokenAprovador == aprovarRequisicaoDto.Token)
                     .FirstOrDefaultAsync();

                    if (requisicaoAprovacao == null || !requisicaoAprovacao.Ativo)
                        throw new Exception("Token inválido e/ou expirado!");

                    //invalidar token da requisicao
                    var requisicaoAprovacoes = await _rm.RequisicaoAprovacaoRepository.SelectByCondition(c => c.TokenRequisicao == requisicaoAprovacao.TokenRequisicao).ToListAsync();

                    foreach (var item in requisicaoAprovacoes)
                    {
                        item.DataRevogacao = DateTime.UtcNow;
                        _rm.RequisicaoAprovacaoRepository.Update(item);
                    }

                    alterarStatus = requisicaoAprovacao.AlteraStatus;
                    requisicaoId = requisicaoAprovacao.RequisicaoId;
                    tipoAprovador = requisicaoAprovacao.TipoAprovador;
                }

                var query = _rm.RequisicaoRepository.SelectByCondition(p => p.Id == requisicaoId);
                var data = await query.FirstOrDefaultAsync();

                if (data == null)
                    return false;

                string status = aprovarRequisicaoDto.Aprovado ? "APROVADA" : "REJEITADA";
                string evento = $"Requisição <b>{status}</b> por {aprovarRequisicaoDto.NomeUsuario}<br/>Comentário: {aprovarRequisicaoDto.Comentario}";

                //verificar se a aprovação é do cliente ou WCA
                if (tipoAprovador == EnumTipoAprovador.WCA)
                    data.RequerAutorizacaoWCA = false;
                else if (tipoAprovador == EnumTipoAprovador.CLIENTE)
                    data.RequerAutorizacaoCliente = false;

                if (aprovarRequisicaoDto.Aprovado == false)
                    data.Status = EnumStatusRequisicao.REJEITADO;
                else
                {
                    /* SOMENTE APROVAR se o status atual for AGUARDANDO
                     *  -- CANCELADO  - NÃO ALTERAR
                     *  -- FINALIZADO - NÃO ALTERAR
                     *  -- REJEITADO  - NÃO ALTERAR
                    */
                    if (data.Status == EnumStatusRequisicao.AGUARDANDO)
                    {
                        // SOMENTE ALTERAR SE:
                        // solicitação por alterar status
                        // não requer aprovação do cliente ou WCA
                        if (alterarStatus &&
                            data.RequerAutorizacaoCliente == false &&
                            data.RequerAutorizacaoWCA == false)
                        {
                            data.Status = EnumStatusRequisicao.APROVADO;
                        }
                    }
                    else
                    {
                        evento = $"APROVAÇÃO realizado por {aprovarRequisicaoDto.NomeUsuario}, NÃO ACATADA, motivo: requisição com status: ";
                        evento += data.Status switch
                        {
                            EnumStatusRequisicao.APROVADO => "APROVADO",
                            EnumStatusRequisicao.CANCELADO => "CANCELADO",
                            EnumStatusRequisicao.FINALIZADO => "FINALIZADO",
                            EnumStatusRequisicao.REJEITADO => "REJEITADO",
                            _ => "DESCONHECIDO"
                        };
                    }
                }
                _rm.RequisicaoRepository.Update(data);

                await _rm.SaveAsync();

                RequisicaoHistorico reqH = new RequisicaoHistorico()
                {
                    RequisicaoId = requisicaoId,
                    Evento = evento,
                    DataHora = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _timeZoneBrasilia)
                };

                await CreateRequisicaoHistorico(reqH);

                //verificar se envia solicitação de aprovação do fornecedor, caso a  aprovação seja por cliente/wca
                if (tipoAprovador != EnumTipoAprovador.FORNECEDOR &&
                    !data.RequerAutorizacaoCliente && !data.RequerAutorizacaoWCA)
                {
                    await solicitarAprovacaoFornecedor(urlOrigin, data);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{this.GetType().Name}.aprovarRequisicao.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public Pagination<RequisicaoDto> Paginate(int filialId, int page = 1, int pageSize = 10, int clienteId = 0, int fornecedorId = 0, int usuarioId = 0, EnumStatusRequisicao status = EnumStatusRequisicao.TODOS, DateTime? dataInicio = null, DateTime? dataFim = null)
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

                if (dataInicio != null && dataFim != null)
                    query = query.Where(c => c.DataCriacao >= dataInicio && c.DataCriacao <= dataFim);


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

        public async Task<bool> Remove(int filialId, int id, string nomeUsuario)
        {
            try
            {
                var query = _rm.RequisicaoRepository.SelectByCondition(p => p.Id == id, true);

                if (filialId > 1)
                    query = query.Where(c => c.FilialId == filialId);

                var data = await query.FirstOrDefaultAsync();

                if (data == null) return false;

                data.Status = EnumStatusRequisicao.CANCELADO;

                await _rm.SaveAsync();

                RequisicaoHistorico reqH = new RequisicaoHistorico()
                {
                    RequisicaoId = data.Id,
                    Evento = $"Requisição <b>CANCELADA</b> por {nomeUsuario}.",
                    DataHora = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _timeZoneBrasilia)
                };

                await CreateRequisicaoHistorico(reqH);



                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{this.GetType().Name}.Remove.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<RequisicaoDto> Update(int filialId, UpdateRequisicaoDto updateRequisicaoDto, string urlOrigin = "")
        {
            try
            {
                var query = _rm.RequisicaoRepository.SelectByCondition(p => p.Id == updateRequisicaoDto.Id);

                if (filialId > 1)
                    query = query.Where(c => c.FilialId == filialId);

                query = query.Include("RequisicaoItens");

                var data = await query.FirstOrDefaultAsync();

                if (data == null) return null;

                if (data.Status == EnumStatusRequisicao.APROVADO || data.Status == EnumStatusRequisicao.CANCELADO || data.Status == EnumStatusRequisicao.FINALIZADO)
                    throw new Exception("Requisição não pode ser editada!");

                //Remover produtos caso tenha alterado
                data.RequisicaoItens.ToList().ForEach(produto =>
                {
                    bool hasProduto = updateRequisicaoDto.RequisicaoItens.Where(p => p.Id == produto.Id).FirstOrDefault() != null;
                    if (hasProduto == false)
                    {
                        var item = _rm.RequisicaoItemRepository.SelectByCondition(c => c.Id == produto.Id).FirstOrDefault();
                        if (item != null) _rm.RequisicaoItemRepository.Delete(item);
                    }
                });
                _mapper.Map(updateRequisicaoDto, data);

                if (updateRequisicaoDto.Status == EnumStatusRequisicao.ITENSALTERADOS)
                {
                    if (updateRequisicaoDto.RequerAutorizacaoCliente == false || updateRequisicaoDto.RequerAutorizacaoWCA == false)
                    {
                        var comprasMes = await GetQuantidadePedidoPorCliente((int)data.ClienteId, DateTime.Now.AddDays(-30), DateTime.Now);

                        var cliente = await _rm.ClienteRepository.SelectByCondition(c => c.Id == data.ClienteId)
                                               .Include(inc => inc.ClienteOrcamentoConfiguracao)
                                               .FirstOrDefaultAsync();

                        foreach (var item in cliente.ClienteOrcamentoConfiguracao)
                        {
                            if (comprasMes[item.TipoFornecimentoId] > item.QuantidadeMes)
                            {
                                if (item.AprovadoPor == EnumAprovadoPor.WCA)
                                    data.RequerAutorizacaoWCA = true;
                                else
                                    data.RequerAutorizacaoCliente = true;
                            }
                        }
                    }
                    data.Status = EnumStatusRequisicao.AGUARDANDO;
                }
                _rm.RequisicaoRepository.Update(data);

                await _rm.SaveAsync();

                await CreateRequisicaoHistorico(new RequisicaoHistorico()
                {
                    DataHora = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _timeZoneBrasilia),
                    Evento = $"Requisição alterada por {updateRequisicaoDto.NomeUsuario}",
                    RequisicaoId = data.Id
                });

                if (updateRequisicaoDto.Status == EnumStatusRequisicao.ITENSALTERADOS)
                {

                    if (data.RequerAutorizacaoCliente == true)
                    {
                        await solicitarAprovacaoCliente(urlOrigin, data);
                    }

                    if (data.RequerAutorizacaoWCA == true)
                    {
                        await solicitarAprovacaoWCA(urlOrigin, data);
                    }

                    if (data.RequerAutorizacaoWCA == false && data.RequerAutorizacaoCliente == false)
                    {
                        await solicitarAprovacaoFornecedor(urlOrigin, data);
                    }
                }

                return _mapper.Map<RequisicaoDto>(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{this.GetType().Name}.Update.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<Stream> ExportToExcel(int requisicaoId)
        {

            try
            {
                var requisicao = await GetById(1, requisicaoId);

                if (requisicao == null)
                {
                    return null;
                }
                //var currentDirectory = Directory.GetCurrentDirectory();
                //var parentDirectory = Directory.GetParent(currentDirectory).FullName;
                //var filesDirectory = Path.Combine(parentDirectory, "Files");
                //filesDirectory = Path.Combine(filesDirectory, "excel");
                var excelFile = "Files/excel/WCAPedidoFornecedor.xlsx"; //Path.Combine(filesDirectory, "WCAPedidoFornecedor.xlsx");

                string localEntrega = $"{requisicao.Endereco}, {requisicao.Numero} - CEP: {requisicao.CEP} - {requisicao.Cidade} / {requisicao.UF}";

                var workbook = new XLWorkbook(excelFile);
                var ws = workbook.Worksheet(1);
                ws.Cell("C1").SetValue(requisicao.Id);
                ws.Cell("C3").SetValue(requisicao.Cliente.Nome);
                ws.Cell("C4").SetValue(requisicao.Cliente.CNPJ);
                ws.Cell("C5").SetValue(localEntrega);
                ws.Cell("C6").SetValue(requisicao.Usuario.Text);

                var row = 9;

                foreach (var item in requisicao.RequisicaoItens)
                {
                    ws.Cell($"A{row}").SetValue(item.Codigo);
                    ws.Cell($"B{row}").SetValue(item.Nome);
                    ws.Cell($"C{row}").SetValue(item.Valor);
                    ws.Cell($"D{row}").SetValue(item.Quantidade);
                    ws.Cell($"E{row}").SetValue(item.PercentualIPI);
                    ws.Cell($"F{row}").SetValue(item.Icms);
                    ws.Cell($"G{row}").SetValue(item.Valor * (1 + item.PercentualIPI / 100));
                    ws.Cell($"H{row}").SetValue((item.Valor * (1 + item.PercentualIPI / 100)) * item.Quantidade);
                    ws.Cell($"I{row}").SetValue(item.TaxaGestao);
                    ws.Cell($"J{row}").SetValue(item.TaxaGestao * item.Quantidade);
                    ws.Cell($"K{row}").SetValue(item.ValorTotal);
                    row++;
                }
                Stream spreadsheetStream = new MemoryStream();
                workbook.SaveAs(spreadsheetStream);
                spreadsheetStream.Position = 0;

                return spreadsheetStream;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"{this.GetType().Name}.ExportExcel.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }

        }

        public async Task<RequisicaoDuplicarResponse> Duplicate(int requisicaoId, int usuarioId, string usuarioNome, string urlOrigin)
        {
            var query = _rm.RequisicaoRepository.SelectByCondition(c => c.Id == requisicaoId);
            query = query.Include(r => r.RequisicaoItens);

            var requisicao = await query.FirstOrDefaultAsync();

            if (requisicao == null)
                return null;


            //Replicar os produtos, somente os que existem na base
            List<RequisicaoItemDto> itens = new List<RequisicaoItemDto>();
            string message = "";
            foreach (var reqItem in requisicao.RequisicaoItens)
            {
                var produto = await _rm.ProdutoRepository.SelectByCondition(c => c.FornecedorId == requisicao.FornecedorId && c.Codigo == reqItem.Codigo).FirstOrDefaultAsync();

                if (produto != null)
                {
                    reqItem.Id = 0;
                    reqItem.RequisicaoId = 0;
                    reqItem.Valor = produto.Valor;
                    reqItem.TaxaGestao = produto.TaxaGestao;
                    reqItem.PercentualIPI = produto.PercentualIPI;
                    reqItem.ValorTotal = (produto.Valor + produto.TaxaGestao + (produto.Valor * produto.PercentualIPI / 100)) * reqItem.Quantidade;
                    RequisicaoItemDto item = _mapper.Map<RequisicaoItemDto>(reqItem);
                    itens.Add(item);
                }
                else
                {
                    message += $"Produto {reqItem.Codigo} - {reqItem.Nome}. <br/>";
                }
            }

            // Criar o pedido
            CreateRequisicaoDto novoPedido = new CreateRequisicaoDto((int)requisicao.FilialId, (int)requisicao.ClienteId,
                (int)requisicao.FornecedorId, requisicao.ValorTotal, requisicao.TaxaGestao, requisicao.Destino,
                requisicao.Endereco, requisicao.Numero, requisicao.Cep, requisicao.Cidade, requisicao.UF, usuarioId,
                usuarioNome, itens, false, false, requisicao.ValorIcms, requisicao.PeriodoEntrega
            );

            var data = await Create(novoPedido, urlOrigin);

            return new RequisicaoDuplicarResponse(data, message);
        }

        public async Task<bool> FinalizarPedido(FinalizarRequisicaoDto finalizarRequisicaoDto, string usuarioNome)
        {

            var requisicao = await _rm.RequisicaoRepository.SelectByCondition(c => c.Id == finalizarRequisicaoDto.Id, true)
                                   .FirstOrDefaultAsync();

            if (requisicao == null)
            {
                return false;
            }

            requisicao.Status = EnumStatusRequisicao.FINALIZADO;
            requisicao.DataEntrega = finalizarRequisicaoDto.DataEntrega;
            requisicao.NotaFiscal = finalizarRequisicaoDto.NotaFiscal;

            await _rm.SaveAsync();

            RequisicaoHistorico reqH = new RequisicaoHistorico()
            {
                RequisicaoId = requisicao.Id,
                Evento = $"Requisição <b>FINALIZADA</b> por {usuarioNome}, Data Entrega: {requisicao.DataEntrega} Nota Fiscal: {requisicao.NotaFiscal}",
                DataHora = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _timeZoneBrasilia)
            };

            await CreateRequisicaoHistorico(reqH);


            return true;
        }

        public async Task<bool> EnviarRequisicao(int requisicaoId, EnumRequisicaoDestinoEmail destino, string urlOrigin)
        {
            try
            {
                Requisicao requisicao = await _rm.RequisicaoRepository.SelectByCondition(c => c.Id == requisicaoId).FirstOrDefaultAsync();

                if (requisicao == null) { return false; }

                if (destino == EnumRequisicaoDestinoEmail.FORNECEDOR)
                    await solicitarAprovacaoFornecedor(urlOrigin, requisicao, false);
                else
                    await solicitarAprovacaoCliente(urlOrigin, requisicao, false);

                return true;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"{this.GetType().Name}.EnviarRequisicao.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }


        public async Task<Stream> ExportToExcel(int filialId, int clienteId, int fornecedorId, int usuarioId, 
            EnumStatusRequisicao status = EnumStatusRequisicao.TODOS,
            DateTime? dataInicio = null, DateTime? dataFim = null)
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
                if (dataInicio !=null && dataFim !=null)
                {
                    query = query.Where(c => c.DataCriacao >= dataInicio && c.DataCriacao<= dataFim);
                }

                query = query.Include("Usuario")
                             .Include("Cliente")
                             .Include("Fornecedor")
                             .Include("RequisicaoItens").OrderBy(o => o.Id);

                var requisicoes = await query.ToListAsync();

                if (requisicoes.Count == 0) return null;


                var excelFile = "Files/excel/modelo_relatorio_requisicoes.xlsx";
                var workbook = new XLWorkbook(excelFile);
                var ws = workbook.Worksheet(1); 
                
                var row = 3;

                foreach (var requisicao in requisicoes)
                {
                    foreach (var item in requisicao.RequisicaoItens)
                    {
                        ws.Cell($"B{row}").SetValue(requisicao.Id);
                        ws.Cell($"C{row}").SetValue(requisicao.Usuario.Nome);
                        ws.Cell($"D{row}").SetValue(requisicao.Cliente.Nome);

                        string localEntrega = $"{requisicao.Endereco}, {requisicao.Numero} - CEP: {requisicao.Cep} - {requisicao.Cidade} / {requisicao.UF}";
                        ws.Cell($"E{row}").SetValue(localEntrega);
                        ws.Cell($"F{row}").SetValue(requisicao.Cliente.CNPJ);

                        ws.Cell($"G{row}").SetValue(item.Nome);
                        ws.Cell($"H{row}").SetValue(item.Quantidade);
                        ws.Cell($"I{row}").SetValue(item.Valor);
                        ws.Cell($"J{row}").SetValue(item.PercentualIPI);
                        ws.Cell($"K{row}").SetValue(item.Icms);
                        ws.Cell($"L{row}").SetValue(item.Valor * (1 + item.PercentualIPI/100));
                        ws.Cell($"M{row}").SetValue((item.Valor * (1 + item.PercentualIPI / 100)) * item.Quantidade);
                        ws.Cell($"N{row}").SetValue(item.TaxaGestao);
                        ws.Cell($"O{row}").SetValue(item.TaxaGestao * item.Quantidade);
                        ws.Cell($"P{row}").SetValue(item.ValorTotal);
                        row++;
                    }
                }

                Stream spreadsheetStream = new MemoryStream();
                workbook.SaveAs(spreadsheetStream);
                spreadsheetStream.Position = 0;

                return spreadsheetStream;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{this.GetType().Name}.ExportToExcel.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }


        #region Private Functions
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

        private async Task solicitarAprovacaoFornecedor(string urlOrigin, Requisicao requisicao, bool checkConfiguracao = true)
        {
            try
            {
                //Verificar se esta configurado para enviar solicitação ao Fornecedor
                if (checkConfiguracao == true)
                {
                    Configuracao? config = _configuracoes.FirstOrDefault(c => c.Chave == "requisicao.sendemail.fornecedor");
                    if (config.Valor == "false") return;

                }

                IList<FornecedorContato> contatos = await _rm.FornecedorContatoRepository.SelectByCondition(c => c.FornecedorId == requisicao.FornecedorId).ToListAsync();

                var requisicaoToken = randomTokenString();

                foreach (var contato in contatos)
                {
                    RequisicaoAprovacao req = new RequisicaoAprovacao()
                    {
                        NomeAprovador = contato.Nome,
                        RequisicaoId = requisicao.Id,
                        TokenRequisicao = requisicaoToken,
                        TokenAprovador = randomTokenString(),
                        AlteraStatus = false,
                        TipoAprovador = EnumTipoAprovador.FORNECEDOR
                    };
                    _rm.RequisicaoAprovacaoRepository.Create(req);
                    await _rm.SaveAsync();

                    var link = $"{urlOrigin}/app/requisicoes/aprovar/{req.TokenAprovador}";

                    var bodyHtml = "<p style='color:grey; text-align: center;font-size:36px'>Você recebeu um novo pedido da WCA.<br/>";
                    bodyHtml += "Clique no botão abaixo para acessar o <br/>";
                    bodyHtml += "pedido<p><br/>";
                    bodyHtml += $"<p style='text-align: center;'><a href='{link}' style='font: bold 18px Arial; text-decoration: none;background-color: #000066; color: white; padding: 1em 1.5em; border-radius: 5%;'>Acessar pedido</a></p>";

                    _emailService.SendRequisicaoParaAprovacao(new string[] { contato.Email }, bodyHtml);
                }

                RequisicaoHistorico reqH = new RequisicaoHistorico()
                {
                    RequisicaoId = requisicao.Id,
                    Evento = $"Solicitação de aprovação enviada ao fornecedor!",
                    DataHora = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _timeZoneBrasilia)
                };

                await CreateRequisicaoHistorico(reqH);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"{this.GetType().Name}.solicitarAprovacaoFornecedor.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        private async Task solicitarAprovacaoWCA(string urlOrigin, Requisicao requisicao)
        {
            try
            {
                var query = _rm.UsuarioRepository.SelectAll()
                            .Include(u => u.Cliente)
                            .Where(c => c.Ativo == true && c.Cliente.Any(c => c.Id == requisicao.ClienteId));

                var usuarios = await query.ToListAsync();
                
                if (usuarios.Count > 0) {
                    var requisicaoToken = randomTokenString();

                    foreach (var contato in usuarios)
                    {
                        RequisicaoAprovacao req = new RequisicaoAprovacao()
                        {
                            NomeAprovador = contato.Nome,
                            RequisicaoId = requisicao.Id,
                            TokenRequisicao = requisicaoToken,
                            TokenAprovador = randomTokenString(),
                            AlteraStatus = true,
                            TipoAprovador = EnumTipoAprovador.WCA
                        };
                        _rm.RequisicaoAprovacaoRepository.Create(req);
                        await _rm.SaveAsync();

                        var link = $"{urlOrigin}/app/requisicoes/aprovar/{req.TokenAprovador}";

                        var bodyHtml = "<p style='color:grey; text-align: center;font-size:36px'>Você recebeu um novo pedido para.<br/>";
                        bodyHtml += "aprovar. Clique no botão abaixo para acessar o <br/>";
                        bodyHtml += "pedido<p><br/>";
                        bodyHtml += $"<p style='text-align: center;'><a href='{link}' style='font: bold 18px Arial; text-decoration: none;background-color: #000066; color: white; padding: 1em 1.5em; border-radius: 5%;'>Acessar pedido</a></p>";

                        _emailService.SendRequisicaoParaAprovacao(new string[] { contato.Email }, bodyHtml);
                    }

                    RequisicaoHistorico reqH = new RequisicaoHistorico()
                    {
                        RequisicaoId = requisicao.Id,
                        Evento = $"Solicitação de aprovação enviada para aprovação do administrador!",
                        DataHora = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _timeZoneBrasilia)
                    };

                    await CreateRequisicaoHistorico(reqH);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{this.GetType().Name}.solicitarAprovacaoFornecedor.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        private async Task solicitarAprovacaoCliente(string urlOrigin, Requisicao requisicao, bool checkConfiguracao = true)
        {
            try
            {
                //Verificar se esta configurado para enviar solicitação ao Fornecedor
                if (checkConfiguracao == true)
                {
                    Configuracao? config = _configuracoes.FirstOrDefault(c => c.Chave == "requisicao.sendemail.cliente");
                    if (config.Valor == "false") return;

                }

                IList<ClienteContato> contatos = await _rm.ClienteContatoRepository.SelectByCondition(c => c.ClienteId == requisicao.ClienteId).ToListAsync();

                var requisicaoToken = randomTokenString();

                foreach (var contato in contatos)
                {
                    RequisicaoAprovacao req = new RequisicaoAprovacao()
                    {
                        NomeAprovador = contato.Nome,
                        RequisicaoId = requisicao.Id,
                        TokenRequisicao = requisicaoToken,
                        TokenAprovador = randomTokenString(),
                        AlteraStatus = true,
                        TipoAprovador = EnumTipoAprovador.CLIENTE
                    };
                    _rm.RequisicaoAprovacaoRepository.Create(req);
                    await _rm.SaveAsync();

                    var link = $"{urlOrigin}/app/requisicoes/aprovar/{req.TokenAprovador}";

                    var bodyHtml = "<p style='color:grey; text-align: center;font-size:36px'>Você recebeu um novo pedido para.<br/>";
                    bodyHtml += "aprovar. Clique no botão abaixo para acessar o <br/>";
                    bodyHtml += "pedido<p><br/>";
                    bodyHtml += $"<p style='text-align: center;'><a href='{link}' style='font: bold 18px Arial; text-decoration: none;background-color: #000066; color: white; padding: 1em 1.5em; border-radius: 5%;'>Acessar pedido</a></p>";

                    _emailService.SendRequisicaoParaAprovacao(new string[] { contato.Email }, bodyHtml);
                }

                RequisicaoHistorico reqH = new RequisicaoHistorico()
                {
                    RequisicaoId = requisicao.Id,
                    Evento = $"Solicitação de aprovação enviada para aprovação do Cliente!",
                    DataHora = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _timeZoneBrasilia)
                };

                await CreateRequisicaoHistorico(reqH);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"{this.GetType().Name}.solicitarAprovacaoFornecedor.Error: {ex.Message}");
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

        private async Task<Dictionary<int, int>> GetQuantidadePedidoPorCliente(int clienteId, DateTime initialDate, DateTime finalDate)
        {
            var query = _rm.RequisicaoRepository
                .SelectByCondition(cnd => cnd.ClienteId == clienteId
                                && cnd.DataCriacao >= initialDate
                                && cnd.DataCriacao <= finalDate)
                .Include("RequisicaoItens");
            var result = await query.ToListAsync();

            Dictionary<int, int> QuantidadePorTipo = new Dictionary<int, int>();

            var categorias = await _rm.TipoFornecimentoRepository.SelectByCondition(c => c.Ativo).ToListAsync();

            foreach (var categoria in categorias)
            {
                QuantidadePorTipo.Add(categoria.Id, 0);
            }

            foreach (var item in result)
            {
                foreach (int key in QuantidadePorTipo.Keys)
                {
                    QuantidadePorTipo[key] += item.RequisicaoItens.Where(c => c.TipoFornecimentoId == key).Count() > 0 ? 1 : 0;
                }
            }
            return QuantidadePorTipo;
        }

        private (DateTime, DateTime) getDataCorte()
        {

            var diaHoje = DateTime.Now.Day;

            DateTime dataCorteIni = DateTime.Now.AddDays(-30);
            DateTime dataCorteFim = DateTime.Now;

            Configuracao? config = _configuracoes.FirstOrDefault(c => c.Chave == "requisicao.datacorte");

            if (config != null)
            {
                var diaCorte = int.Parse(config.Valor);

                if (diaHoje > diaCorte)
                {
                    dataCorteIni = new DateTime(DateTime.Now.Year, DateTime.Now.Month, diaCorte);
                }
                else
                {
                    if (DateTime.Now.Month == 1)
                    {
                        dataCorteIni = new DateTime(DateTime.Now.Year - 1, DateTime.Now.Month - 1, diaCorte);
                    }
                    else
                    {
                        dataCorteIni = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, diaCorte);
                    }
                }
            }

            return (dataCorteIni, dataCorteFim);
        }

        #endregion
    }
}
