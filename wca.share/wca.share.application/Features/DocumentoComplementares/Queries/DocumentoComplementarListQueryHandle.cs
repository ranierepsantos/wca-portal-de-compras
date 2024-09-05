using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using wca.share.application.Common;
using wca.share.application.Contracts.Persistence;
using wca.share.domain.Entities;

namespace wca.share.application.Features.DocumentoComplementares.Queries
{
    public record DocumentoComplementarListQuery() : IRequest<ErrorOr<IList<ListItem>>>;

    internal class DocumentoComplementarListQueryHandle : IRequestHandler<DocumentoComplementarListQuery, ErrorOr<IList<ListItem>>>
    {
        private readonly IRepositoryManager _repository;
        private readonly ILogger<DocumentoComplementarListQueryHandle> _logger;
        private readonly IMapper _mapper;

        public DocumentoComplementarListQueryHandle(IRepositoryManager repository, ILogger<DocumentoComplementarListQueryHandle> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ErrorOr<IList<ListItem>>> Handle(DocumentoComplementarListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                List<DocumentoComplementar>? items = await _repository.GetDbSet<DocumentoComplementar>()
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
