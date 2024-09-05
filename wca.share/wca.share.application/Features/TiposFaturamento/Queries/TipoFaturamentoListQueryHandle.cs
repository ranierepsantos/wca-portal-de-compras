using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.share.application.Common;
using wca.share.application.Contracts.Persistence;
using wca.share.domain.Entities;

namespace wca.share.application.Features.TiposFaturamento.Queries
{
    public record TipoFaturamentoListQuery() : IRequest<ErrorOr<IList<ListItem>>>;

    internal class TipoFaturamentoListQueryHandle : IRequestHandler<TipoFaturamentoListQuery, ErrorOr<IList<ListItem>>>
    {
        private readonly IRepositoryManager _repository;
        private readonly ILogger<TipoFaturamentoListQueryHandle> _logger;
        private readonly IMapper _mapper;

        public TipoFaturamentoListQueryHandle(IRepositoryManager repository, ILogger<TipoFaturamentoListQueryHandle> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ErrorOr<IList<ListItem>>> Handle(TipoFaturamentoListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                List<TipoFaturamento>? items = await _repository.GetDbSet<TipoFaturamento>()
                                            .AsNoTracking()
                                            .Where(q => q.Ativo)
                                            .OrderBy(x => x.Nome)
                                            .ToListAsync(cancellationToken: cancellationToken);

                return _mapper.Map<List<ListItem>>(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Error.Failure("", ex.Message);
            }
        }
    }
}
