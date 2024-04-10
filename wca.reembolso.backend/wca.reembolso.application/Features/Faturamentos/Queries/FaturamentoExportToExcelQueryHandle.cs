using AutoMapper;
using ClosedXML.Excel;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Common;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.Faturamentos.Queries
{

    public record FaturamentoExportToExcelQuery(
        DateTime? DataIni,
        DateTime? DataFim,
        int? FilialId = 0,
        int? Status = 0,
        int[]? ClienteIds = null,
        int[]? CentroCustoIds = null
    ) : IRequest<ErrorOr<Stream>>;
    public sealed class FaturamentoExportToExcelQueryHandle : IRequestHandler<FaturamentoExportToExcelQuery, ErrorOr<Stream>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<FaturamentoExportToExcelQueryHandle> _logger;

        public FaturamentoExportToExcelQueryHandle(IRepositoryManager repository, IMapper mapper, ILogger<FaturamentoExportToExcelQueryHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<Stream>> Handle(FaturamentoExportToExcelQuery request, CancellationToken cancellationToken)
        {
            if (request.DataIni > request.DataFim || (request.DataIni != null && request.DataFim is null) || (request.DataFim != null && request.DataIni is null))
            {
                return Error.Validation("Data início ou fim inválida!");
            }

            var query = _repository.FaturamentoRepository.ToQuery();
            query = query.Include(i => i.Cliente)
                         .Include(i => i.CentroCusto)
                         .Include(n => n.FaturamentoItem)
                         .ThenInclude(n => n.Solicitacao)
                         .ThenInclude(n => n.Colaborador)
                         .Include(n => n.FaturamentoItem)
                         .ThenInclude(faturamentoItem => faturamentoItem.Solicitacao);
                         //.ThenInclude(solicitacao => solicitacao.Gestor);

            if (request.FilialId > 1)
                query = query.Where(q => q.Cliente.FilialId.Equals(request.FilialId));

            if (request.ClienteIds?.Length > 0)
                query = query.Where(q => request.ClienteIds.Contains(q.ClienteId));

            if (request.CentroCustoIds?.Length > 0)
                query = query.Where(q => request.CentroCustoIds.Contains(q.CentroCustoId));
            
            if (request.Status > 0)
                query = query.Where(q => q.Status.Equals(request.Status));

            if (request.DataIni != null && request.DataFim != null)
            {
                var dataFim = request.DataFim.Value.AddHours(23).AddMinutes(59);
                query = query.Where(c => c.DataCriacao >= request.DataIni && c.DataCriacao <= dataFim);
            }
                
            query = query.OrderBy(q => q.Id);

            List<Faturamento> dados = await query.ToListAsync();

            if (dados.Count == 0) return Error.NotFound(description: "Sem dados para exportar!");

            /*
                { id: 1, status: "Aguardando P.O", color: "warning", notifica: 2, autorizar: false, templateNotificacao:"Faturamento #{id}, aguardando P.O!" },
                { id: 2, status: "P.O Emitido", color: "success", notifica: 1, autorizar: false, templateNotificacao:"Faturamento #{id}, P.O emitido!" },
                { id: 3, status: "Finalizado", color: "success", notifica: "", autorizar: false, templateNotificacao:"" },
             */
            var workBook = new XLWorkbook();
            var sheet = workBook.AddWorksheet("Faturamentos");
            var sheetDespesas = workBook.AddWorksheet("Faturamento_Solicitacoes");

            var numberFormat = "#,##0.00; (#,##0.00)";
            int row = 2;
            int rowDespesa = 2;
            //montar o header do relatório

            sheet.Cell($"B{row}").SetValue("Código");
            sheet.Cell($"C{row}").SetValue("Data Faturamento");
            sheet.Cell($"D{row}").SetValue("Cliente");
            sheet.Cell($"E{row}").SetValue("Centro de Custo");
            sheet.Cell($"F{row}").SetValue("Status");
            sheet.Cell($"G{row}").SetValue("Valor");
            sheet.Cell($"H{row}").SetValue("P.O.");
            sheet.Cell($"I{row}").SetValue("Nota Fiscal");



            sheetDespesas.Cell($"B{row}").SetValue("Código");
            sheetDespesas.Cell($"C{row}").SetValue("Data Faturamento");
            sheetDespesas.Cell($"D{row}").SetValue("Cliente");
            sheetDespesas.Cell($"E{row}").SetValue("Centro de Custo");
            sheetDespesas.Cell($"F{row}").SetValue("Status");
            sheetDespesas.Cell($"G{row}").SetValue("Valor");
            sheetDespesas.Cell($"H{row}").SetValue("P.O.");
            sheetDespesas.Cell($"I{row}").SetValue("Nota Fiscal");

            sheetDespesas.Cell($"J{row}").SetValue("Código Solicitação");
            sheetDespesas.Cell($"K{row}").SetValue("Colaborador");
            sheetDespesas.Cell($"L{row}").SetValue("Centro de Custo");
            sheetDespesas.Cell($"M{row}").SetValue("Tipo Solicitação");
            sheetDespesas.Cell($"N{row}").SetValue("Valor Despesa");

            
            row++;
            rowDespesa++;
            foreach (var item in dados)
            {
                sheet.Cell($"B{row}").SetValue(item.Id).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sheet.Cell($"C{row}").SetValue(item.DataCriacao).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sheet.Cell($"D{row}").SetValue(item.Cliente.Nome);
                sheet.Cell($"E{row}").SetValue(item.CentroCusto?.Nome);
                sheet.Cell($"F{row}").SetValue(item.Status switch
                {
                    1 => "AGUARDANDO P.O.",
                    2 => "P.O. EMITIDO",
                    3 => "FINALIZADO",
                    _ => $"{item.Status} - DESCONHECIDO"
                });
                sheet.Cell($"G{row}").SetValue(item.Valor).Style.NumberFormat.Format = numberFormat;
                sheet.Cell($"H{row}").SetValue(item.NumeroPO);
                sheet.Cell($"I{row}").SetValue(item.NotaFiscal);

                foreach (var faturamentoItem in item.FaturamentoItem)
                {

                    sheetDespesas.Cell($"B{rowDespesa}").SetValue(sheet.Cell($"B{row}").Value).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sheetDespesas.Cell($"C{rowDespesa}").SetValue(sheet.Cell($"C{row}").Value).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sheetDespesas.Cell($"D{rowDespesa}").SetValue(sheet.Cell($"D{row}").Value);
                    sheetDespesas.Cell($"E{rowDespesa}").SetValue(sheet.Cell($"E{row}").Value);

                    sheetDespesas.Cell($"F{rowDespesa}").SetValue(sheet.Cell($"F{row}").Value);
                    sheetDespesas.Cell($"G{rowDespesa}").SetValue(sheet.Cell($"G{row}").Value).Style.NumberFormat.Format = numberFormat;
                    sheetDespesas.Cell($"H{rowDespesa}").SetValue(sheet.Cell($"H{row}").Value);
                    sheetDespesas.Cell($"I{rowDespesa}").SetValue(sheet.Cell($"I{row}").Value);

                    sheetDespesas.Cell($"J{rowDespesa}").SetValue(faturamentoItem.Solicitacao.Id).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sheetDespesas.Cell($"K{rowDespesa}").SetValue(faturamentoItem.Solicitacao.Colaborador?.Nome);
                    sheetDespesas.Cell($"L{rowDespesa}").SetValue(faturamentoItem.Solicitacao.CentroCusto?.Nome);
                    sheetDespesas.Cell($"M{rowDespesa}").SetValue(faturamentoItem.Solicitacao.TipoSolicitacao ==1 ? "REEMBOLSO": "ADIANTAMENTO");
                    sheetDespesas.Cell($"N{rowDespesa}").SetValue(faturamentoItem.Solicitacao.ValorDespesa).Style.NumberFormat.Format = numberFormat;

                    rowDespesa++;
                }
                row++;
            }

            sheet.Range("B2:I2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sheet.Range("B2:I2").Style.Fill.BackgroundColor = XLColor.BlueGray;
            sheet.Range("B2:I2").Style.Font.Bold = true;

            sheetDespesas.Range("B2:N2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sheetDespesas.Range("B2:N2").Style.Fill.BackgroundColor = XLColor.BlueGray;
            sheetDespesas.Range("B2:N2").Style.Font.Bold = true;

            Stream spreadsheetStream = new MemoryStream();
            workBook.SaveAs(spreadsheetStream);
            spreadsheetStream.Position = 0;

            return spreadsheetStream;
        }
    }
}
    
    
