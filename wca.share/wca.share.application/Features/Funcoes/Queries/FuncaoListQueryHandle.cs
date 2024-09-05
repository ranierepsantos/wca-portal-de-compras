using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.share.application.Common;
using wca.share.application.Contracts.Persistence;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Funcoes.Queries
{
    public record FuncaoListQuery() : IRequest<ErrorOr<IList<ListItem>>>;

    internal class FuncaoListQueryHandle : IRequestHandler<FuncaoListQuery, ErrorOr<IList<ListItem>>>
    {
        private readonly IRepositoryManager _repository;
        private readonly ILogger<FuncaoListQueryHandle> _logger;
        private readonly IMapper _mapper;

        public FuncaoListQueryHandle(IRepositoryManager repository, ILogger<FuncaoListQueryHandle> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ErrorOr<IList<ListItem>>> Handle(FuncaoListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                List<Funcao>? items = await _repository.GetDbSet<Funcao>()
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
