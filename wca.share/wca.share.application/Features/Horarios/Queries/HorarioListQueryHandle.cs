using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.share.application.Common;
using wca.share.application.Contracts.Persistence;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Horarios.Queries
{
    public record HorarioListQuery() : IRequest<ErrorOr<IList<ListItem>>>;

    internal class HorarioListQueryHandle : IRequestHandler<HorarioListQuery, ErrorOr<IList<ListItem>>>
    {
        private readonly IRepositoryManager _repository;
        private readonly ILogger<HorarioListQueryHandle> _logger;
        private readonly IMapper _mapper;

        public HorarioListQueryHandle(IRepositoryManager repository, ILogger<HorarioListQueryHandle> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ErrorOr<IList<ListItem>>> Handle(HorarioListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                List<Horario>? items = await _repository.GetDbSet<Horario>()
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
