using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.share.application.Common;
using wca.share.application.Contracts.Persistence;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Supervisors.Queries
{
    public record SupervisorListQuery() : IRequest<ErrorOr<IList<ListItem>>>;

    internal class SupervisorListQueryHandle : IRequestHandler<SupervisorListQuery, ErrorOr<IList<ListItem>>>
    {
        private readonly IRepositoryManager _repository;
        private readonly ILogger<SupervisorListQueryHandle> _logger;
        private readonly IMapper _mapper;

        public SupervisorListQueryHandle(IRepositoryManager repository, ILogger<SupervisorListQueryHandle> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ErrorOr<IList<ListItem>>> Handle(SupervisorListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                List<Supervisor>? items = await _repository.GetDbSet<Supervisor>()
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
