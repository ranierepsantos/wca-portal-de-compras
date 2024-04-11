using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.domain.Entities;

namespace wca.reembolso.application.Features.Solicitacoes.Commands
{
    public sealed record SolicitacaoCheckVencidoCommand() : IRequest<ErrorOr<string>>;
    internal sealed class SolicitacaoCheckVencidoCommandHandle : IRequestHandler<SolicitacaoCheckVencidoCommand, ErrorOr<string>>
    {
        private readonly IRepositoryManager _repository;
        private readonly ILogger<SolicitacaoCheckVencidoCommandHandle> _logger;
        private readonly IMediator _mediator;

        public SolicitacaoCheckVencidoCommandHandle(IRepositoryManager repository, ILogger<SolicitacaoCheckVencidoCommandHandle> logger, IMediator mediator)
        {
            _repository = repository;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<ErrorOr<string>> Handle(SolicitacaoCheckVencidoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string sql = "SELECT id,cliente_id,data_solicitacao,colaborador_id,CentroCustoId,colaborador_cargo," +
                              "       projeto,objetivo,periodo_inicial,periodo_final,valor_adiantamento,valor_despesa," +
                              "       tipo_solicitacao,StatusSolicitacaoId,StatusAnteriorId,data_status " +
                              "FROM Solicitacoes (nolock) " +
                              "WHERE StatusSolicitacaoId = 3" +
                              "  AND DATEDIFF(day, data_status, getdate()) > 30";

                List<Solicitacao>? solicitacoes = await _repository.GetFromSQL<Solicitacao>(sql);

                _logger.LogInformation($"Quantidade de solicitações encontradas: {solicitacoes?.Count}");

                var novoStatus = await _repository.StatusSolicitacaoRepository.ToQuery()
                                        .Where(q => q.Id == 12)
                                        .FirstAsync(cancellationToken: cancellationToken);

                foreach(Solicitacao item in solicitacoes)
                {
                    var comand = new SolicitacaoChangeStatusCommand(
                        item.Id,
                        "Status alterado para <b>VENCIDO</b> pelo sistema!",
                        novoStatus,
                        new int[] { item.ColaboradorId }
                    );
                    await _mediator.Send(comand);
                }

                return $"{solicitacoes?.Count} alteradas!";
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception.message" + ex.Message);
                if (ex.InnerException is not null)
                    _logger.LogError("Exception.InnerException" + ex.InnerException?.Message);
                
                return Error.Failure(description: ex.Message);
            }
            
        }
    }
}
