using wca.share.domain.Entities;

namespace wca.share.application.Contracts
{
    public interface INotificacaoHandle
    {
        Task SolicitacaoEnviarNotificacaoAsync(int[] usersId, string template, Solicitacao solicitacao, CancellationToken cancellationToken = default);

    }
}
