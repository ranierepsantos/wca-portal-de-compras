using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.share.application.Common;
using wca.share.application.Contracts.Persistence;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Escalas.Queries
{
    public record EscalaListQuery() : IRequest<ErrorOr<IList<ListItem>>>;

    internal class EscalaListQueryHandle : IRequestHandler<EscalaListQuery, ErrorOr<IList<ListItem>>>
    {
        private readonly IRepositoryManager _repository;
        private readonly ILogger<EscalaListQueryHandle> _logger;
        private readonly IMapper _mapper;

        public EscalaListQueryHandle(IRepositoryManager repository, ILogger<EscalaListQueryHandle> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ErrorOr<IList<ListItem>>> Handle(EscalaListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                List<Escala>? items = await _repository.GetDbSet<Escala>()
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
