using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.SolicitacaoHistoricos.Commands
{
    public record SolicitacaoHistorioCreateCommand(
        int SolicitacaoId,
        string Evento
    ): IRequest<ErrorOr<bool>>;
    public sealed class SolicitacaoHistoricoCreateCommandHandler : IRequestHandler<SolicitacaoHistorioCreateCommand, ErrorOr<bool>>
    {
        private readonly IRepository<SolicitacaoHistorico> _repository;
        private readonly ILogger<SolicitacaoHistoricoCreateCommandHandler> _logger;

        public SolicitacaoHistoricoCreateCommandHandler(IRepository<SolicitacaoHistorico> repository, ILogger<SolicitacaoHistoricoCreateCommandHandler> logger)
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

            _repository.Create( evento );

            await _repository.SaveChangesAsync();

            return true;
        }
    }
}
