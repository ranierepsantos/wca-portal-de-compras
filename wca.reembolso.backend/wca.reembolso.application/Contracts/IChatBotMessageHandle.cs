using wca.reembolso.application.Features.Faturamentos.Common;
using wca.reembolso.application.Features.Solicitacoes.Common;

namespace wca.reembolso.application.Contracts.Integration
{
    internal interface IChatBotMessageHandle
    {
        Task SolicitacaoSendMessageAsync(int[] usersId, SolicitacaoResponse solicitacao, CancellationToken cancellationToken = default);
        Task FaturamentoSendMessageAsync(int[] usersId, FaturamentoChatBot faturamento, bool firstSend = false, CancellationToken cancellationToken = default);
        Task FaturamentoSendMessageAsync(int[] usersId, int faturamentoId, CancellationToken cancellationToken = default);
        
        Task DepositoSendMessageAsync(SolicitacaoResponse solicitacao, DateTime? datadeposito, decimal? valordeposito, CancellationToken cancellationToken);
    }
}
