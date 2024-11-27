using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.share.application.Common;
using wca.share.application.Contracts.Persistence;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Escolaridades.Queries
{
    public record EscolaridadeListQuery() : IRequest<ErrorOr<IList<ListItem>>>;

    internal class EscolaridadeListQueryHandle : IRequestHandler<EscolaridadeListQuery, ErrorOr<IList<ListItem>>>
    {
        private readonly IRepositoryManager _repository;
        private readonly ILogger<EscolaridadeListQueryHandle> _logger;
        private readonly IMapper _mapper;

        public EscolaridadeListQueryHandle(IRepositoryManager repository, ILogger<EscolaridadeListQueryHandle> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ErrorOr<IList<ListItem>>> Handle(EscolaridadeListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                List<Escolaridade>? items = await _repository.GetDbSet<Escolaridade>()
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
