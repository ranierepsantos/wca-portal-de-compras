using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.share.application.Contracts.Persistence;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Solicitacoes.Queries
{
    public record StatusSolicitacaoListQuery() : IRequest<ErrorOr<IList<StatusSolicitacao>>>;
    internal sealed class StatusSolicitacaoListQueryHandle : IRequestHandler<StatusSolicitacaoListQuery, ErrorOr<IList<StatusSolicitacao>>>
    {
        private readonly IRepositoryManager _repository;
        private readonly ILogger<StatusSolicitacaoListQueryHandle> _logger;

        public StatusSolicitacaoListQueryHandle(IRepositoryManager repository, ILogger<StatusSolicitacaoListQueryHandle> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ErrorOr<IList<StatusSolicitacao>>> Handle(StatusSolicitacaoListQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Status Solicitação - GetAll");

            var query = _repository.GetDbSet<StatusSolicitacao>()
                        .AsQueryable()
                        .AsNoTracking()
                        .OrderBy(o => o.Status);

            return await query.ToListAsync(cancellationToken: cancellationToken);

        }
    }
}
