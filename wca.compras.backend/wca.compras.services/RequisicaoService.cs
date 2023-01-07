using AutoMapper;
using ClosedXML.Excel;
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
                await solicitarAprovacaoFornecedor(urlOrigin, data);

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
                             .Include(r => r.Cliente)
                             .ThenInclude(c =>  c.ClienteOrcamentoConfiguracao)
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

                var dto = new RequisicaoAprovacaoDto(
                    data.Id, 
                    data.ValorTotal, 
                    data.TaxaGestao, 
                    data.Destino,
                    data.Cliente,
                    _mapper.Map<ListItem>(data.Usuario), 
                    _mapper.Map<IList<RequisicaoItemDto>>(data.RequisicaoItens), 
                    requisicaoAprovacao.NomeAprovador
                );
                return dto;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{this.GetType().Name}.GetByAprovacaoToken.Error: {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<bool> aprovarRequisicao(AprovarRequisicaoDto aprovarRequisicaoDto) 
        {
            try
            {
                var requisicaoAprovacao = await _rm.RequisicaoAprovacaoRepository
                     .SelectByCondition(c => c.TokenAprovador == aprovarRequisicaoDto.Token)
                     .FirstOrDefaultAsync();

                if (requisicaoAprovacao == null || !requisicaoAprovacao.Ativo)
                    throw new Exception("Token inválido e/ou expirado!");

                var query = _rm.RequisicaoRepository.SelectByCondition(p => p.Id == requisicaoAprovacao.RequisicaoId);
                var data = await query.FirstOrDefaultAsync();

                if (data == null)
                    return false;

                if (aprovarRequisicaoDto.Aprovado == false)
                    data.Status = EnumStatusRequisicao.REJEITADO;
                else
                    data.Status = requisicaoAprovacao.AlteraStatus? EnumStatusRequisicao.APROVADO: data.Status;

                _rm.RequisicaoRepository.Update(data);
                
                //invalidar token da requisicao
                var requisicaoAprovacoes = await _rm.RequisicaoAprovacaoRepository.SelectByCondition(c => c.TokenRequisicao == requisicaoAprovacao.TokenRequisicao).ToListAsync();

                foreach(var item in requisicaoAprovacoes)
                {
                    item.DataRevogacao = DateTime.UtcNow;
                    _rm.RequisicaoAprovacaoRepository.Update(item);
                }

                await _rm.SaveAsync();

                string status = aprovarRequisicaoDto.Aprovado ? "APROVADA" : "REJEITADA";

                RequisicaoHistorico reqH = new RequisicaoHistorico()
                {
                    RequisicaoId = requisicaoAprovacao.RequisicaoId,
                    Evento = $"Requisição <b>{status}</b> por {requisicaoAprovacao.NomeAprovador}<br/>Comentário: {aprovarRequisicaoDto.Comentario}",
                    DataHora = DateTime.Now
                };

                await CreateRequisicaoHistorico(reqH);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{this.GetType().Name}.GetByAprovacaoToken.Error: {ex.Message}");
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

        public async Task<bool> Remove(int filialId, int id, string nomeUsuario)
        {
            try
            {
                var query = _rm.RequisicaoRepository.SelectByCondition(p => p.Id == id,true);

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
                    DataHora = DateTime.Now
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

        public async Task<Stream> ExportToExcel(int requisicaoId)
        {

            try
            {
                var requisicao = await GetById(1, requisicaoId);

                if (requisicao == null)
                {
                    return null;
                }
                var currentDirectory = Directory.GetCurrentDirectory();
                var parentDirectory = Directory.GetParent(currentDirectory).FullName;
                var filesDirectory = Path.Combine(parentDirectory, "Files");
                filesDirectory = Path.Combine(filesDirectory, "excel");
                var excelFile = Path.Combine(filesDirectory, "WCAPedidoFornecedor.xlsx");

                var saveFile = $"WCAPedido_{requisicaoId}.xlsx";


                var workbook = new XLWorkbook(excelFile);
                var ws = workbook.Worksheet(1);
                ws.Cell("C1").SetValue(requisicao.Id);
                ws.Cell("C3").SetValue(requisicao.Cliente.Nome);
                ws.Cell("C4").SetValue(requisicao.Cliente.CNPJ);
                ws.Cell("C5").SetValue(requisicao.Cliente.Endereco);
                ws.Cell("C6").SetValue(requisicao.Usuario.Text);

                var row = 9;

                foreach (var item in requisicao.RequisicaoItens)
                {
                    ws.Cell($"A{row}").SetValue(item.Codigo);
                    ws.Cell($"B{row}").SetValue(item.Nome);
                    ws.Cell($"C{row}").SetValue(item.Valor);
                    ws.Cell($"D{row}").SetValue(item.Quantidade);
                    ws.Cell($"E{row}").SetValue(item.UnidadeMedida);
                    ws.Cell($"F{row}").SetValue(item.ValorTotal);
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

        private async Task solicitarAprovacaoFornecedor (string urlOrigin, Requisicao requisicao)
        {
            try
            {
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
                        AlteraStatus = true
                    };
                    _rm.RequisicaoAprovacaoRepository.Create(req);
                    await _rm.SaveAsync();

                    var link = $"{urlOrigin}/app/requisicoes/aprovar/{req.TokenAprovador}";
                    _emailService.SendRequisicaoFornecedorEmail(new string[] { contato.Email }, link);
                }

                RequisicaoHistorico reqH = new RequisicaoHistorico()
                {
                    RequisicaoId = requisicao.Id,
                    Evento = $"Solicitação de aprovação enviada ao fornecedor!",
                    DataHora = DateTime.Now
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
        #endregion
    }
}
