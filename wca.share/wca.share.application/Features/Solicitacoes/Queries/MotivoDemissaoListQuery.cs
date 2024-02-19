using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.share.application.Contracts.Persistence;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Solicitacoes.Queries
{

    public record MotivoDemissaoListQuery() : IRequest<ErrorOr<IList<MotivoDemissao>>>;
    internal sealed class MotivoDemissaoListQueryHandle : IRequestHandler<MotivoDemissaoListQuery, ErrorOr<IList<MotivoDemissao>>>
    {
        private readonly IRepositoryManager _repository;
        private readonly ILogger<MotivoDemissaoListQueryHandle> _logger;

        public MotivoDemissaoListQueryHandle(IRepositoryManager repository, ILogger<MotivoDemissaoListQueryHandle> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ErrorOr<IList<MotivoDemissao>>> Handle(MotivoDemissaoListQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Listando motivos de demissão");

            var query = _repository.GetDbSet<MotivoDemissao>()
                        .AsQueryable()
                        .AsNoTracking()
                        .OrderBy(o => o.Motivo);

            return await query.ToListAsync(cancellationToken: cancellationToken);

        }
    }
}
