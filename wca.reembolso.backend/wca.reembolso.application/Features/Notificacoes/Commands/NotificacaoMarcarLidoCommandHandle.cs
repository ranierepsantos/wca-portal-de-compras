using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Contracts.Persistence;

namespace wca.reembolso.application.Features.Notificacoes.Commands
{
    public sealed record NotificacaoMarcarLidoCommand(int Id): IRequest<ErrorOr<bool>>;
    public sealed class NotificacaoMarcarLidoCommandHandle : IRequestHandler<NotificacaoMarcarLidoCommand, ErrorOr<bool>>
    {
        private readonly IRepositoryManager _repository;
        private readonly ILogger<NotificacaoMarcarLidoCommandHandle> _logger;

        public NotificacaoMarcarLidoCommandHandle(IRepositoryManager repository, ILogger<NotificacaoMarcarLidoCommandHandle> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ErrorOr<bool>> Handle(NotificacaoMarcarLidoCommand request, CancellationToken cancellationToken)
        {

            var notificacao = await _repository.NotificacaoRepository.ToQuery().FirstOrDefaultAsync(q => q.Id == request.Id, cancellationToken: cancellationToken);
                                    
            if (notificacao == null)
            {
                string logTexto = "Notificação não localizada!";
                _logger.LogError(logTexto);
                return Error.NotFound(description: logTexto);
            }

            notificacao.Lido = true;
            _repository.NotificacaoRepository.Update(notificacao);

            await _repository.SaveAsync();

            return true;

        }
    }
}
