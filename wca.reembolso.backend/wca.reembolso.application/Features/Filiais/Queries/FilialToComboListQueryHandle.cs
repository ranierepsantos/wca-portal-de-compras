using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.Filiais.Common;
using wca.reembolso.application6.Common;

namespace wca.reembolso.application.Features.Filiais.Queries
{
    public record FilialToComboListQuerie() : IRequest<ErrorOr<IList<ListItem>>>;

    public sealed class FilialToComboListQueryHandle : IRequestHandler<FilialToComboListQuerie, ErrorOr<IList<ListItem>>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<FilialToComboListQueryHandle> _logger;

        public FilialToComboListQueryHandle(IRepositoryManager repository, IMapper mapper, ILogger<FilialToComboListQueryHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<IList<ListItem>>> Handle(FilialToComboListQuerie request, CancellationToken cancellationToken)
        {

            _logger.LogInformation("Buscando filiais para lista/combo");

            var query = _repository.FilialRepository.ToQuery();

            var items = await query.Where(q => q.Ativo).OrderBy(q => q.Nome).ToListAsync(cancellationToken: cancellationToken);

            return _mapper.Map<List<ListItem>>(items);

        }
    }
}
