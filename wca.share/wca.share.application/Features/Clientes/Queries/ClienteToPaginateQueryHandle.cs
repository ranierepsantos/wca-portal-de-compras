using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using wca.share.application.Common;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Clientes.Common;

namespace wca.share.application.Features.Clientes.Queries
{

    public record ClientePaginateQuery(string Termo = "") : PaginationQuery, IRequest<ErrorOr<Pagination<ClienteResponse>>>;
    public sealed class ClienteToPaginateQueryHandle : 
        IRequestHandler<ClientePaginateQuery, ErrorOr<Pagination<ClienteResponse>>>
    {
        private IRepositoryManager _repository;
        private IMapper _mapper;
        private ILogger<ClienteToPaginateQueryHandle> _logger;
        public ClienteToPaginateQueryHandle(
            IRepositoryManager repository, IMapper mapper, ILogger<ClienteToPaginateQueryHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<Pagination<ClienteResponse>>> Handle(
            ClientePaginateQuery request, CancellationToken cancellationToken)
        {
            var query = _repository.ClienteRepository.ToQuery();

            if (request.FilialId > 1)
                query = query.Where(q => q.FilialId.Equals(request.FilialId));

            if (!string.IsNullOrEmpty(request.Termo))
                query = query.Where(q => q.Nome.Contains(request.Termo));

            query = query.OrderBy(o => o.Nome).AsQueryable();

            var pagination = Pagination<ClienteResponse>.ToPagedList(_mapper, query, request.Page, request.PageSize);

            return await Task.FromResult(pagination) ;
        }
    }
}
