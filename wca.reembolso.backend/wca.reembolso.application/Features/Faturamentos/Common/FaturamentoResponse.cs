using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.Faturamentos.Common
{
    public sealed record FaturamentoPaginateResponse
    (
        int Id,
        int UsuarioId,
        DateTime DataCriacao,
        int ClienteId,
        string ClienteNome,
        int Status,
        decimal Valor,
        int? CentroCustoId,
        string? CentroCustoNome,
        string? NumeroPO,
        string? DocumentoPO,
        string? NotaFiscal,
        string? NumeroDS,
        IList<FaturamentoHistorico>? FaturamentoHistorico
    );

    public sealed record FaturamentoResponse
    (
        int Id,
        DateTime DataCriacao,
        int UsuarioId,
        int ClienteId,
        string ClienteNome,
        int Status,
        decimal Valor,
        int? CentroCustoId,
        string? CentroCustoNome,
        string? NumeroPO,
        string? DocumentoPO,
        DateTime? DataFinalizacao,
        string? NotaFiscal,
        string? NumeroDS,
        IList<FaturamentoItemResponse>? FaturamentoItem,
        IList<FaturamentoHistorico>? FaturamentoHistorico
    );

    public record FaturamentoItemResponse(
        int Id,
        int FaturamentoId, 
        int SolicitacaoId,
        Solicitacao2Faturamento Solicitacao
    );

    public record Solicitacao2Faturamento (
        int Id,
        DateTime DataSolicitacao,
        int ColaboradorId,
        string ColaboradorNome,
        int CentroCustoId,
        decimal ValorDespesa
    );


    public sealed class FaturamentoChatBot
    {
        public int Id { get; init; }
        public DateTime DataCriacao { get; init; }
        public string ClienteNome { get; init; }
        public int Status { get; init; }
        public decimal Valor { get; init; }
        public string CentroCustoNome { get; init; }
        public DateTime DataMenorDespesa { get; init; }
        public DateTime DataMaiorDespesa { get; init; }
    }

}
