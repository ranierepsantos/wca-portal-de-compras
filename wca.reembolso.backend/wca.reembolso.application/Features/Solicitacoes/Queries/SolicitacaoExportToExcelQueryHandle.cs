using ErrorOr;
using MediatR;
using ClosedXML.Excel;
using AutoMapper;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.Solicitacaos.Queries;
using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Spreadsheet;
using wca.reembolso.domain.Entities;
using wca.reembolso.domain.Common.Enum;

namespace wca.reembolso.application.Features.Solicitacoes.Queries
{
    public record SolicitacaoExportToExcelQuery(
        DateTime? DataIni, 
        DateTime? DataFim,
        int FilialId = 1,
        int UsuarioId = 0, 
        int ClienteId = 0, 
        int Status = 0) : IRequest<ErrorOr<Stream>>;

    public class SolicitacaoExportToExcelQueryHandle :
        IRequestHandler<SolicitacaoExportToExcelQuery, ErrorOr<Stream>>
    {
        private readonly IRepositoryManager _repository;
        private readonly ILogger<SolicitacaoExportToExcelQueryHandle> _logger;

        public SolicitacaoExportToExcelQueryHandle(IRepositoryManager repository, ILogger<SolicitacaoExportToExcelQueryHandle> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ErrorOr<Stream>> Handle(SolicitacaoExportToExcelQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Exportando solicitações para planilha excel...");

            if (request.DataIni > request.DataFim || (request.DataIni != null && request.DataFim is null) || (request.DataFim != null && request.DataIni is null))
            {
                return Error.Validation("Data início ou fim inválida!");
            }

            var query = _repository.SolicitacaoRepository.ToQuery();
            query = query.Include(i => i.Cliente)
                         .Include(q => q.Colaborador)
                         .Include(q => q.Gestor)
                         .Include(q => q.Despesa)
                         .ThenInclude(x =>  x.TipoDespesa);

            if (request.FilialId > 1)
                query = query.Where(q => q.Cliente.FilialId.Equals(request.FilialId));

            if (request.UsuarioId > 0)
                query = query.Where(q => q.ColaboradorId.Equals(request.UsuarioId) || q.GestorId.Equals(request.UsuarioId));

            if (request.ClienteId > 0)
                query = query.Where(q => q.ClienteId.Equals(request.ClienteId));

            if (request.Status > 0)
                query = query.Where(q => q.Status.Equals(request.Status));

            if (request.DataIni != null && request.DataFim != null)
            {
                var dataFim = request.DataFim.Value.AddHours(23).AddMinutes(59);
                _logger.LogInformation($"DataFim {dataFim}");
                query = query.Where(c => c.DataSolicitacao >= request.DataIni && c.DataSolicitacao <= dataFim);
            }


            query = query.OrderBy(q => q.Id);

            List<Solicitacao> dados = await query.ToListAsync(cancellationToken: cancellationToken);

            if (dados.Count == 0)
            {
                _logger.LogInformation("Sem dados para exportar!");
                return Error.NotFound(description: "Sem dados para exportar!");
            }

            List<StatusSolicitacao> statusList = await _repository.StatusSolicitacaoRepository.ToQuery().OrderBy(q => q.Id).ToListAsync();

            var workBook = new XLWorkbook();
            var sheet = workBook.AddWorksheet("Solicitacoes");
            var sheetDespesas = workBook.AddWorksheet("Solicitacoes_Despesas");
            
            var numberFormat = "#,##0.00; (#,##0.00)";
            int row = 2;
            int rowDespesa = 2;
            //montar o header do relatório

            sheet.Cell($"B{row}").SetValue("Código");
            sheet.Cell($"C{row}").SetValue("Data Solicitação");
            sheet.Cell($"D{row}").SetValue("Cliente");
            sheet.Cell($"E{row}").SetValue("Colaborador");
            sheet.Cell($"F{row}").SetValue("Gestor");
            sheet.Cell($"G{row}").SetValue("Tipo Solicitação");
            sheet.Cell($"H{row}").SetValue("Status");
            sheet.Cell($"I{row}").SetValue("Valor Adiantamento");
            sheet.Cell($"J{row}").SetValue("Valor Despesa");

            sheetDespesas.Cell($"B{row}").SetValue("Código");
            sheetDespesas.Cell($"C{row}").SetValue("Data Solicitação");
            sheetDespesas.Cell($"D{row}").SetValue("Cliente");
            sheetDespesas.Cell($"E{row}").SetValue("Colaborador");
            sheetDespesas.Cell($"F{row}").SetValue("Gestor");
            sheetDespesas.Cell($"G{row}").SetValue("Tipo Solicitação");
            sheetDespesas.Cell($"H{row}").SetValue("Status");
            sheetDespesas.Cell($"I{row}").SetValue("Valor Adiantamento");
            sheetDespesas.Cell($"J{row}").SetValue("Valor Despesa");
            sheetDespesas.Cell($"K{row}").SetValue("Data Despesa");
            sheetDespesas.Cell($"L{row}").SetValue("Tipo Despesa");
            sheetDespesas.Cell($"M{row}").SetValue("Tipo");
            sheetDespesas.Cell($"N{row}").SetValue("CNPJ");
            sheetDespesas.Cell($"O{row}").SetValue("Razão Social");
            sheetDespesas.Cell($"P{row}").SetValue("Nro Fiscal");
            sheetDespesas.Cell($"Q{row}").SetValue("KM Percorrido");
            sheetDespesas.Cell($"R{row}").SetValue("Valor");

            row++;
            rowDespesa++;
            foreach (var item in dados)
            {
                sheet.Cell($"B{row}").SetValue(item.Id).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                sheet.Cell($"C{row}").SetValue(item.DataSolicitacao).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center); ;
                sheet.Cell($"D{row}").SetValue(item.Cliente.Nome);
                sheet.Cell($"E{row}").SetValue(item.Colaborador?.Nome);
                sheet.Cell($"F{row}").SetValue(item.Gestor?.Nome);
                sheet.Cell($"G{row}").SetValue(item.TipoSolicitacao switch
                {
                    1 => "Reembolso",
                    2 => "Adiantamento",
                    _ => $"{item.TipoSolicitacao}  - Desconhecido"

                } );
                sheet.Cell($"H{row}").SetValue(statusList.Where(q => q.Id == item.Status).First().Status);
                sheet.Cell($"I{row}").SetValue(item.ValorAdiantamento).Style.NumberFormat.Format = numberFormat;
                sheet.Cell($"J{row}").SetValue(item.ValorDespesa).Style.NumberFormat.Format = numberFormat; 
                
                foreach(var despesa in item.Despesa)
                {
                    sheetDespesas.Cell($"B{rowDespesa}").SetValue(item.Id).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    sheetDespesas.Cell($"C{rowDespesa}").SetValue(item.DataSolicitacao).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center); ;
                    sheetDespesas.Cell($"D{rowDespesa}").SetValue(item.Cliente.Nome);
                    sheetDespesas.Cell($"E{rowDespesa}").SetValue(item.Colaborador?.Nome);
                    sheetDespesas.Cell($"F{rowDespesa}").SetValue(item.Gestor?.Nome);
                    sheetDespesas.Cell($"G{rowDespesa}").SetValue(item.TipoSolicitacao switch
                    {
                        1 => "Reembolso",
                        2 => "Adiantamento",
                        _ => $"{item.TipoSolicitacao}  - Desconhecido"

                    });
                    sheetDespesas.Cell($"H{rowDespesa}").SetValue(statusList.Where(q => q.Id == item.Status).First().Status);
                    sheetDespesas.Cell($"I{rowDespesa}").SetValue(item.ValorAdiantamento).Style.NumberFormat.Format = numberFormat;
                    sheetDespesas.Cell($"J{rowDespesa}").SetValue(item.ValorDespesa).Style.NumberFormat.Format = numberFormat;
                    sheetDespesas.Cell($"K{row}").SetValue(despesa.DataEvento);
                    sheetDespesas.Cell($"L{row}").SetValue(despesa.TipoDespesa?.Nome);
                    sheetDespesas.Cell($"M{row}").SetValue(despesa.TipoDespesa?.Tipo == EnumTipoDespesaTipo.Consumo ? "Consumo": "Distância");
                    sheetDespesas.Cell($"N{row}").SetValue(despesa.CNPJ);
                    sheetDespesas.Cell($"O{row}").SetValue(despesa.RazaoSocial);
                    sheetDespesas.Cell($"P{row}").SetValue(despesa.NumeroFiscal);
                    sheetDespesas.Cell($"Q{row}").SetValue(despesa.KmPercorrido == 0 ? "": despesa.KmPercorrido);
                    sheetDespesas.Cell($"R{row}").SetValue(despesa.Valor).Style.NumberFormat.Format = numberFormat; 

                    rowDespesa++;
                }
                row++;
            }
            
            sheet.Range("B2:J2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sheet.Range("B2:J2").Style.Fill.BackgroundColor = XLColor.BlueGray;
            sheet.Range("B2:J2").Style.Font.Bold = true;

            sheetDespesas.Range("B2:R2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            sheetDespesas.Range("B2:R2").Style.Fill.BackgroundColor = XLColor.BlueGray;
            sheetDespesas.Range("B2:R2").Style.Font.Bold = true;

            Stream spreadsheetStream = new MemoryStream();
            workBook.SaveAs(spreadsheetStream);
            spreadsheetStream.Position = 0;

            return spreadsheetStream;
        }
    }
}
