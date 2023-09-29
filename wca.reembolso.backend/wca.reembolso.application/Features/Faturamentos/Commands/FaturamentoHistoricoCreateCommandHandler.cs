using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.Faturamentos.Commands;

public record FaturamentoHistoricoCreateCommand(
    int FaturamentoId,
    string Evento
) : IRequest<ErrorOr<bool>>;
public sealed class FaturamentoHistoricoCreateCommandHandler : IRequestHandler<FaturamentoHistoricoCreateCommand, ErrorOr<bool>>
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<FaturamentoHistoricoCreateCommandHandler> _logger;

    public FaturamentoHistoricoCreateCommandHandler(IRepositoryManager repository, ILogger<FaturamentoHistoricoCreateCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<ErrorOr<bool>> Handle(FaturamentoHistoricoCreateCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Lançando Evento para o Faturamento - {request.FaturamentoId}");

        var evento = new FaturamentoHistorico()
        {
            FaturamentoId = request.FaturamentoId,
            Evento = request.Evento
        };

        _repository.FaturamentoHistoricoRepository.Create(evento);

        await _repository.SaveAsync();

        return true;
    }
}
