using wca.reembolso.application.Features.Solicitacoes.Common;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Contracts
{
    internal interface IChatBotMessageHandle
    {
        Task SolicitacaoSendMessageAsync(int[] usersId, SolicitacaoResponse solicitacao, CancellationToken cancellationToken = default);
        Task FaturamentoSendMessageAsync(int[] usersId, Faturamento faturamento, CancellationToken cancellationToken = default);
    }
}
