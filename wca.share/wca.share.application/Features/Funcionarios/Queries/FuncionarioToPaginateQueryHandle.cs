using AutoMapper;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using wca.share.application.Common;
using wca.share.application.Contracts.Persistence;
using wca.share.application.Features.Funcionarios.Common;
using wca.share.domain.Entities;

namespace wca.share.application.Features.Funcionarios.Queries
{
    public record FuncionarioToPaginateQuery(
        string Termo = "", 
        int[]? ClienteIds = null, 
        int[]? CentroCustoIds = null
    ) : PaginationQuery, IRequest<ErrorOr<Pagination<FuncionarioToPaginateResponse>>>;
    internal class FuncionarioToPaginateQueryHandle : IRequestHandler<FuncionarioToPaginateQuery, ErrorOr<Pagination<FuncionarioToPaginateResponse>>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<FuncionarioToPaginateQueryHandle> _logger;

        public FuncionarioToPaginateQueryHandle(IRepositoryManager repository, IMapper mapper, ILogger<FuncionarioToPaginateQueryHandle> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ErrorOr<Pagination<FuncionarioToPaginateResponse>>> Handle(FuncionarioToPaginateQuery request, CancellationToken cancellationToken)
        {
            string message = $"Parâmetros: {JsonSerializer.Serialize(request)}";
            _logger.LogInformation(message);

            var query = _repository.GetDbSet<Funcionario>().AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(request.Termo))
                query = query.Where(q => q.Nome.Contains(request.Termo));
           
            if (request.ClienteIds?.Length > 0)
                query = query.Where(q => request.ClienteIds.Contains(q.ClienteId));
            
            if (request.CentroCustoIds?.Length > 0)
                query = query.Where(q => request.CentroCustoIds.Contains(q.CentroCustoId));

            if (request.FilialId > 0)
                query = query.Where(q => q.Cliente.FilialId == request.FilialId);

            query = query.Include(o => o.Cliente)
                         .Include(o => o.CentroCusto)
                         .OrderBy(o => o.Nome);

            var pagination = Pagination<FuncionarioToPaginateResponse>.ToPagedList(_mapper, query, request.Page, request.PageSize);

            return await Task.FromResult(pagination);

        }
    }
}
