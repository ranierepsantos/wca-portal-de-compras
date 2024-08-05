using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using wca.share.application.Contracts.Persistence;
using wca.share.domain.Entities;

namespace wca.share.application.Features.VagaHistoricos.Commands
{
    public record VagaHistorioCreateCommand(
        int VagaId,
        string Evento
    ) : IRequest<ErrorOr<bool>>;

    public sealed class VagaHistoricoCreateCommandHandle : IRequestHandler<VagaHistorioCreateCommand, ErrorOr<bool>>
    {
        private readonly IRepositoryManager _repository;
        private readonly ILogger<VagaHistoricoCreateCommandHandle> _logger;

        public VagaHistoricoCreateCommandHandle(IRepositoryManager repository, ILogger<VagaHistoricoCreateCommandHandle> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ErrorOr<bool>> Handle(VagaHistorioCreateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Lançando Evento na Vaga {request.VagaId}");

                var evento = new VagaHistorico()
                {
                    VagaId = request.VagaId,
                    Evento = request.Evento
                };

                _repository.GetDbSet<VagaHistorico>().Add(evento);

                await _repository.SaveAsync();

                return true;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error.Message: {ex.Message}");
                _logger.LogError($"Error.InnerException: {ex.InnerException?.Message}");
                return Error.Failure(ex.Message);
            }

        }
    }
}
