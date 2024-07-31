using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.share.application.Common;
using wca.share.application.Contracts.Persistence;
using wca.share.domain.Entities;

namespace wca.share.application.Features.TipoContratos.Queries
{
    public record TipoContratoListQuery() : IRequest<ErrorOr<IList<ListItem>>>;

    internal class TipoContratoListQueryHandle : IRequestHandler<TipoContratoListQuery, ErrorOr<IList<ListItem>>>
    {
        private readonly IRepositoryManager _repository;
        private readonly ILogger<TipoContratoListQueryHandle> _logger;
        private readonly IMapper _mapper;

        public TipoContratoListQueryHandle(IRepositoryManager repository, ILogger<TipoContratoListQueryHandle> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ErrorOr<IList<ListItem>>> Handle(TipoContratoListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                List<TipoContrato>? items = await _repository.GetDbSet<TipoContrato>()
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
