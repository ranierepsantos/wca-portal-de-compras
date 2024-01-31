using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using wca.share.application.Contracts.Persistence;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Notificacoes.Commands
{
    public sealed record NotificacaoCreateCommand(
        int UsuarioId,
        string Nota,
        string Entidade,
        int EntidadeId
    ): IRequest<ErrorOr<bool>>;

    public sealed class NotificacaoCreateCommandHandle : IRequestHandler<NotificacaoCreateCommand, ErrorOr<bool>>
    {
        private readonly IRepositoryManager _repository;
        
        private readonly ILogger<NotificacaoCreateCommandHandle> _logger;

        public NotificacaoCreateCommandHandle(IRepositoryManager repository, ILogger<NotificacaoCreateCommandHandle> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ErrorOr<bool>> Handle(NotificacaoCreateCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Adicionando notificação");

            Notificacao notificacao = new()
            {
                Nota = request.Nota,
                UsuarioId = request.UsuarioId,
                Entidade = request.Entidade,
                EntidadeId= request.EntidadeId
            };

            _repository.NotificacaoRepository.Create(notificacao);

            await _repository.SaveAsync();
            
            return true;
        }
    }
}
