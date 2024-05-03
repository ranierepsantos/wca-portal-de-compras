using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.share.application.Contracts.Persistence;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Solicitacoes.Queries
{

    public record TipoFeriasListQuery() : IRequest<ErrorOr<IList<TipoFerias>>>;
    internal sealed class TipoFeriasListQueryHandle : IRequestHandler<TipoFeriasListQuery, ErrorOr<IList<TipoFerias>>>
    {
        private readonly IRepositoryManager _repository;
        private readonly ILogger<TipoFeriasListQueryHandle> _logger;

        public TipoFeriasListQueryHandle(IRepositoryManager repository, ILogger<TipoFeriasListQueryHandle> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ErrorOr<IList<TipoFerias>>> Handle(TipoFeriasListQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Listando motivos de demissão");

            var query = _repository.GetDbSet<TipoFerias>()
                        .AsQueryable()
                        .AsNoTracking()
                        .OrderBy(o => o.Descricao);

            return await query.ToListAsync(cancellationToken: cancellationToken);

        }
    }
}
