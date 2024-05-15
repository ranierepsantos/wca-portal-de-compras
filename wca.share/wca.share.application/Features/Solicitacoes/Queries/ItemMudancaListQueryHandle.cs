using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.share.application.Common;
using wca.share.application.Contracts.Persistence;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Solicitacoes.Queries
{
    public record ItemMudancaListQuery () : IRequest<ErrorOr<IList<ListItem>>>;

    internal class ItemMudancaListQueryHandle : IRequestHandler<ItemMudancaListQuery, ErrorOr<IList<ListItem>>>
    {
        private readonly IRepositoryManager _repository;
        private readonly ILogger<ItemMudancaListQueryHandle> _logger;
        private readonly IMapper _mapper;

        public ItemMudancaListQueryHandle(IRepositoryManager repository, ILogger<ItemMudancaListQueryHandle> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ErrorOr<IList<ListItem>>> Handle(ItemMudancaListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                List<ItemMudanca>? items = await _repository.GetDbSet<ItemMudanca>()
                                            .AsNoTracking()
                                            .Where(q => q.Ativo)
                                            .OrderBy(x =>  x.Descricao)
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
