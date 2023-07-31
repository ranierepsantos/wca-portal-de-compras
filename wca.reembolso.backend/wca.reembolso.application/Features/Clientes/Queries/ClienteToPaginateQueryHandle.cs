using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using wca.reembolso.application.Common;
using wca.reembolso.application.Contracts.Persistence;
using wca.reembolso.application.Features.Clientes.Common;

namespace wca.reembolso.application.Features.Clientes.Queries
{

    public record ClientePaginateQuery (
        int FilialId = 1,
        int Page = 2,
        int PageSize = 10,
        string Termo = ""
    ): IRequest<ErrorOr<Pagination<ClienteResponse>>>;
    public sealed class ClienteToPaginateQueryHandle : 
        IRequestHandler<ClientePaginateQuery, ErrorOr<Pagination<ClienteResponse>>>
    {
        private IClienteRepository _reposistory;
        private IMapper _mapper;
        private ILogger<ClienteToPaginateQueryHandle> _logger;
        public ClienteToPaginateQueryHandle(
            IClienteRepository reposistory, IMapper mapper, ILogger<ClienteToPaginateQueryHandle> logger)
        {
            _reposistory = reposistory;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<Pagination<ClienteResponse>>> Handle(
            ClientePaginateQuery request, CancellationToken cancellationToken)
        {
            var query = _reposistory.ToQuery();

            if (!string.IsNullOrEmpty(request.Termo))
                query = query.Where(q => q.Nome.Contains(request.Termo));

            var pagination = Pagination<ClienteResponse>.ToPagedList(_mapper, query, request.Page, request.PageSize);

            return await Task.FromResult(pagination) ;
        }
    }
}
