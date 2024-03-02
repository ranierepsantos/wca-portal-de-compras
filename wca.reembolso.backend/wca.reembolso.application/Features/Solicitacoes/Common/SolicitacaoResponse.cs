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
        string? ColaboradorNome,
        int? CentroCustoId,
        string? CentroCustoNome,
        string ColaboradorCargo,
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
        IList<Despesa> Despesa,
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
        public IList<SolicitacaoHistorico> SolicitacaoHistorico { get; init; }
    }
}
