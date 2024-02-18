using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using wca.share.application.Contracts.Persistence;
using wca.share.domain.Entities;

namespace wca.share.application.Features.SolicitacaoHistoricos.Commands
{
    public record SolicitacaoHistorioCreateCommand(
        int SolicitacaoId,
        string Evento
    ): IRequest<ErrorOr<bool>>;
    public sealed class SolicitacaoHistoricoCreateCommandHandler : IRequestHandler<SolicitacaoHistorioCreateCommand, ErrorOr<bool>>
    {
        private readonly IRepositoryManager _repository;
        private readonly ILogger<SolicitacaoHistoricoCreateCommandHandler> _logger;

        public SolicitacaoHistoricoCreateCommandHandler(IRepositoryManager repository, ILogger<SolicitacaoHistoricoCreateCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ErrorOr<bool>> Handle(SolicitacaoHistorioCreateCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Lançando Evento na solicitação {request.SolicitacaoId}");

            var evento = new SolicitacaoHistorico()
            {
                SolicitacaoId = request.SolicitacaoId,
                Evento = request.Evento
            };

            _repository.SolicitacaoHistoricoRepository.Create( evento );

            await _repository.SaveAsync();

            return true;
        }
    }
}
