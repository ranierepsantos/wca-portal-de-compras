using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.Solicitacoes.Common
{
    public record SolicitacaoResponse(
        int Id,
        int ClienteId,
        DateTime DataSolicitacao,
        int ColaboradorId,
        int GestorId,
        string ColaboradorCargo,
        string Projeto,
        string Objetivo,
        DateTime? PeriodoInicial,
        DateTime? PeriodoFinal,
        decimal ValorAdiantamento,
        decimal ValorDespesa,
        int Status,
        IList<Despesa> Despesa,
        IList<SolicitacaoHistorico> SolicitacaoHistorico
    );
}
