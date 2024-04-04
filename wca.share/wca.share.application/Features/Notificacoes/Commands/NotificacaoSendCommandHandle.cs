using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Contracts;
using wca.share.application.Contracts.Persistence;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Notificacoes.Commands
{
    public sealed record NotificacaoSendCommand(
        int UsuarioId,
        string Nota,
        string Entidade,
        int EntidadeId
    ): IRequest<ErrorOr<bool>>;

    public sealed class NotificacaoSendCommandHandle : IRequestHandler<NotificacaoSendCommand, ErrorOr<bool>>
    {
        private readonly IRepositoryManager _repository;
        private readonly INotificacaoHandle _notificacaoHandle;
        private readonly ILogger<NotificacaoSendCommandHandle> _logger;

        public NotificacaoSendCommandHandle(IRepositoryManager repository, ILogger<NotificacaoSendCommandHandle> logger, INotificacaoHandle notificacaoHandle)
        {
            _repository = repository;
            _logger = logger;
            _notificacaoHandle = notificacaoHandle;
        }

        public async Task<ErrorOr<bool>> Handle(NotificacaoSendCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Enviando notificação {JsonSerializer.Serialize(request)}");

            Solicitacao? solicitacao = await _repository.SolicitacaoRepository
                                                        .ToQuery()
                                                        .Where(q => q.Id.Equals(request.EntidadeId))
                                                        .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if ( solicitacao is not null)
            {
                await _notificacaoHandle.SolicitacaoEnviarNotificacaoAsync(new int[] { request.UsuarioId }, request.Nota, solicitacao, cancellationToken);
            }
            
            return true;
        }
    }
}
