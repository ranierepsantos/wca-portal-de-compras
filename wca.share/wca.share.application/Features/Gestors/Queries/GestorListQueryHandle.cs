using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.share.application.Common;
using wca.share.application.Contracts.Persistence;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Gestors.Queries
{
    public record GestorListQuery() : IRequest<ErrorOr<IList<ListItem>>>;

    internal class GestorListQueryHandle : IRequestHandler<GestorListQuery, ErrorOr<IList<ListItem>>>
    {
        private readonly IRepositoryManager _repository;
        private readonly ILogger<GestorListQueryHandle> _logger;
        private readonly IMapper _mapper;

        public GestorListQueryHandle(IRepositoryManager repository, ILogger<GestorListQueryHandle> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ErrorOr<IList<ListItem>>> Handle(GestorListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                List<Gestor>? items = await _repository.GetDbSet<Gestor>()
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
