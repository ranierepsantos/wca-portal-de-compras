using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using wca.reembolso.application.Common;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.Filiais.Common;

namespace wca.reembolso.application.Features.Filiais.Queries
{
    public record FilialPaginateQuery(string Termo = "") : PaginationQuery, IRequest<ErrorOr<Pagination<FilialResponse>>>;
    public sealed class FilialToPaginateQueryHandle :
        IRequestHandler<FilialPaginateQuery, ErrorOr<Pagination<FilialResponse>>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<FilialToPaginateQueryHandle> _logger;
        public FilialToPaginateQueryHandle(
            IRepositoryManager repository, IMapper mapper, ILogger<FilialToPaginateQueryHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<Pagination<FilialResponse>>> Handle(FilialPaginateQuery request, CancellationToken cancellationToken)
        {
            var query = _repository.FilialRepository.ToQuery();

            if (!string.IsNullOrEmpty(request.Termo))
                query = query.Where(q => q.Nome.Contains(request.Termo));

            var pagination = Pagination<FilialResponse>.ToPagedList(_mapper, query, request.Page, request.PageSize);

            return await Task.FromResult(pagination);
        }
    }
}
