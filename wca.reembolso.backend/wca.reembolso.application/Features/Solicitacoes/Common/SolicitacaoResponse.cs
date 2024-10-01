using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using wca.reembolso.application.Features.Clientes.Common;
using wca.reembolso.domain.Common.Enum;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.Solicitacoes.Common
{
    public record SolicitacaoResponse(
        int Id,
        int ClienteId,
        DateTime DataSolicitacao,
        int ColaboradorId,
        string ColaboradorCargo,
        string? ColaboradorNome,
        int? CentroCustoId,
        string? CentroCustoNome,
        string? ColaboradorCelular,
        string Projeto,
        string Objetivo,
        DateTime? PeriodoInicial,
        DateTime? PeriodoFinal,
        decimal ValorAdiantamento,
        decimal ValorDespesa,
        int Status,
        int StatusAnterior,
        int TipoSolicitacao,
        ClienteResponse Cliente,
        string? Descricao,
        DateTime? DataEntrega,
        DateTime? DataPrevistaEntrega,
        string? Marca,
        decimal? ValorUnitario,
        decimal? ValorFrete,
        int? Quantidade,
        IList<DespesaResponse> Despesa,
        IList<SolicitacaoHistorico> SolicitacaoHistorico
    );


    public class SolicitacaoToPaginateResponse {
        public int Id { get; init; }
        public DateTime DataSolicitacao { get; init; }
        public string? ClienteNome { get; init; }
        public string? ColaboradorNome { get; init; }
        public string? CentroCustoNome { get; init; }
        public decimal ValorAdiantamento { get; init; }
        public decimal ValorDespesa { get; init; }
        public int Status { get; init; }
        public int TipoSolicitacao { get; init; }
        public DateTime? DataMenorDespesa { get; init; }
        public DateTime? DataMaiorDespesa { get; init; }
        public Decimal? ValorFaturavelCliente { get; init; }
        public Decimal? ValorReembolsoColaborador { get; init; }
        public IList<SolicitacaoHistorico> SolicitacaoHistorico { get; init; }
    }

    public sealed class DespesaResponse
    {
        public int Id { get; init; }
        public int SolicitacaoId { get; init; }
        public DateTime? DataEvento { get; init; }
        public int TipoDespesaId { get; init; }
        public string? TipoDespesaNome { get; init; } 
        public string? NumeroFiscal { get; init; } 
        public decimal Valor { get; init; }
        public string? ImagePath { get; init; } 
        public string? RazaoSocial { get; init; } 
        public string? CNPJ { get; init; } 
        public string? InscricaoEstadual { get; init; } 
        public string? Motivo { get; init; } 
        public string? Origem { get; init; } 
        public string? Destino { get; init; } 
        public decimal KmPercorrido { get; init; }
        public int Aprovada { get; init; }
        public string? Observacao { get; init; } 
        public bool ReembolsarColaborador { get; init; }
        public bool FaturarCliente { get; init; }
    }

}
